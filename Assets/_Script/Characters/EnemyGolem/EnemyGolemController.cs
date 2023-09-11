using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DarkTonic.MasterAudio;
using PathologicalGames;


public class EnemyGolemController : MonoBehaviour
{
    public int golemfLife = 30;
    public GameObject panda;
    public float distanceToPanda; // расстояние до панды
    public float minSpeed = 14;
    public float maxSpeed = 16;
    RichAI golemAi; // ии голем
    public Animator animGolem; // антиматор
    bool isAtaka; // если да голем атакует
    //public bool isAtakaFromPanda; // если да голем атакует панда
    public float timerIsAtakaFromPanda; // отсчет времени когда голему снова позволено атаковать
    public bool isDeadGolem; // если да голем умер

    
    [Space(5)]
    public GameObject uiTextDamage;
    public GameObject vfxHit;
    public GameObject vfxPowerAttackStone; // атака голема камнем из под земли

    [Space(5)]
    private float timerBlockingAttack;
    public GameObject uiHealthSliderGolem;
    public GameObject uiManaSliderGolem;
    public int powerAttack = -5;
    
    int golemManaMax = 100;
    float golemManaCurrent = 0;
    bool isGolemManaFull; //если да мана полная доступна мощная атака
    
    void Awake()
    {
        panda = GameObject.FindWithTag("Panda");
        
        golemAi = GetComponent<RichAI>();
        golemAi.maxSpeed = Random.Range(minSpeed, maxSpeed);
        uiHealthSliderGolem.GetComponent<SetSliderBar>().SetMaxValueSlider(golemfLife);
        uiManaSliderGolem.GetComponent<SetSliderBar>().SetMaxValueSlider(golemManaMax);
        uiManaSliderGolem.GetComponent<SetSliderBar>().SetCurrentValue(0);
    }


    void Update()
    {
        timerBlockingAttack += Time.deltaTime;

        if(isDeadGolem)
        {
            return; // прерываем выполнение Update - голем мертв
        }

        timerIsAtakaFromPanda += Time.deltaTime;
        distanceToPanda = Vector3.Distance(transform.position, panda.transform.position);
        //print(distanceToPanda);
        

        if(distanceToPanda < 2.3f && timerIsAtakaFromPanda > 0.8f){ // атака  
            print("Golem TimerAtt" + timerIsAtakaFromPanda);
            isAtaka = true;
            golemAi.canMove = false;
            animGolem.SetBool("walk", false);
            if(timerBlockingAttack > 0.8f) // если истек таймер атакуем
            {   if(!isGolemManaFull) // обычная атака
                {
                    timerIsAtakaFromPanda = 0;
                    timerBlockingAttack = 0;
                    animGolem.SetTrigger("attack1");
                    MasterAudio.PlaySound("GolemAttack1", 1.0f, null, 0.4f);
                    panda.GetComponent<PandaHealth>().SetPandaHealt(powerAttack);
                }else // мощная атака 
                {
                    timerIsAtakaFromPanda = 0;
                    timerBlockingAttack = 0;
                    animGolem.SetTrigger("comboB");
                    //MasterAudio.PlaySound("GolemAttack1", 1.0f, null, 0.4f);
                    //panda.GetComponent<PandaHealth>().SetPandaHealt(powerAttack);
                    Invoke("PowerAttack", 1.0f); // запуск атаки с задержкой
                    isGolemManaFull = false;
                    golemManaCurrent = 0;
                    uiManaSliderGolem.GetComponent<SetSliderBar>().SetCurrentValue((int)golemManaCurrent);
                    MasterAudio.PlaySound("GolemAttackPower");
                }
            }
        }else if(animGolem.GetCurrentAnimatorStateInfo(0).IsName("idle")){
            golemAi.canMove = true;
            isAtaka = false;
            if(golemAi.canMove)// анимация бега
            {
                animGolem.SetBool("walk", true);
            }else
            {
                animGolem.SetBool("walk", false);
            }
        }

        
        // добавляем ману голему
        golemManaCurrent += Time.deltaTime * 3f;
        uiManaSliderGolem.GetComponent<SetSliderBar>().SetCurrentValue((int)golemManaCurrent);
        if(golemManaCurrent >= golemManaMax) 
            isGolemManaFull = true;

    }


    public void AttackFromPanda(int damage) // атака сто стороны панды
    {
        if(isDeadGolem)
            return; 

        if(golemfLife <= 0) // смерть голем
        {
            GolemDead();
        }else{ // удар по голему
            uiManaSliderGolem.SetActive(true);
            uiHealthSliderGolem.SetActive(true);
            golemManaCurrent += 4;
            uiHealthSliderGolem.GetComponent<SetSliderBar>().SetCurrentValue(golemfLife -= damage);
            if(!golemAi.canMove)
                animGolem.SetTrigger("TakeDamage"); // анимация повреждения только если голем стоит
            MasterAudio.PlaySound("SwordToTarget_woolf");

            // ниже очки повреждения через pool manager
            Transform myInst = PoolManager.Pools["uiDamageScore"].Spawn(uiTextDamage.transform);
            myInst.GetComponent<UiTextDamage>()?.GoAnim(damage);
            myInst.transform.position = transform.position + new Vector3(0, 5.0f, 0);
            PoolManager.Pools["uiDamageScore"].Despawn(myInst, 3);

            // ниже эффект VFX для ускорения через pool manager
            Transform myInstHit = PoolManager.Pools["poolVFX"].Spawn(vfxHit);
            //myInstHit.transform.position = transform.position; // приаттачиваем к панде
            myInstHit.transform.SetParent(transform);
            myInstHit.localPosition = Vector3.zero;
            PoolManager.Pools["poolVFX"].Despawn(myInstHit, 3);
        }
    }

    void GolemDead()
    { // смерть голема
        print("DeadGolem");
        //gameObject.layer = 0;
        //panda.GetComponent<Detector>().ReleaseDetection(gameObject);
        //GetComponent<DetectableObject>().DetectionReleased(gameObject);
        animGolem.SetBool("Run", false);
        animGolem.SetTrigger("death");            
        uiHealthSliderGolem.SetActive(false);
        uiManaSliderGolem.SetActive(false);
        GetComponent<CharacterController>().enabled = false;
        GetComponent<RichAI>().radius = 0.01f;
        GetComponent<RichAI>().enabled = false;
        MasterAudio.PlaySound("SwordToTarget_woolf");
        MasterAudio.PlaySound("GolemDeath");
        golemAi.canMove = false;
        isDeadGolem = true;
        panda.GetComponent<PandaHealth>().SetPandaMana(100); // добавляем ману панде
    }



    void PowerAttack() // мощная атака камень из под земли
    {
        // ниже эффект VFX для RMB через pool manager
        Transform myInstGolem = PoolManager.Pools["poolVFX"].Spawn(vfxPowerAttackStone);
        myInstGolem.transform.position = panda.transform.position;

        PoolManager.Pools["poolVFX"].Despawn(myInstGolem, 6);
    }
}
