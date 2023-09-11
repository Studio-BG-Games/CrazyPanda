using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DarkTonic.MasterAudio;
using PathologicalGames;


public class EnemyWoolfController : MonoBehaviour
{
    public int woolfLife = 30;
    public GameObject panda;
    public float distanceToPanda; // расстояние до панды
    public float minSpeed = 14;
    public float maxSpeed = 16;
    EnemyWoolfAi woolfAi; // ии волка
    public Animator animWoolf; // антиматор
    bool isAtaka; // если да волк атакует
    //public bool isAtakaFromPanda; // если да волка атакует панда
    public float timerIsAtakaFromPanda; // отсчет времени когда волку снова позволено атаковать
    public bool isDeadWoolf; // если да волк умер

    private bool isSlowMotion; // если да включен режим замедления

    [Space(5)]
    public GameObject uiTextDamage;
    public GameObject vfxHit;

    [Space(5)]
    private float timerBlockingAttack;
    public GameObject uiHealthSlider;
    public int powerAttack = -5;
    
    
    void Awake()
    {
        panda = GameObject.FindWithTag("Panda");
        animWoolf = GetComponent<Animator>();
        woolfAi = GetComponent<EnemyWoolfAi>();
        woolfAi.maxSpeed = Random.Range(minSpeed, maxSpeed);
        uiHealthSlider.GetComponent<SetSliderBar>().SetMaxValueSlider(woolfLife);
    }


    void Update()
    {
        timerBlockingAttack += Time.deltaTime;

        if(isDeadWoolf)
        {
            return; // прерываем выполнение Update - волк мертв
        }

        timerIsAtakaFromPanda += Time.deltaTime;
        distanceToPanda = Vector3.Distance(transform.position, panda.transform.position);
        //print(distanceToPanda);
        

        if(distanceToPanda < 2.3f && timerIsAtakaFromPanda > 0.8f){ // атака  woolfAi.remainingDistance
        //print("WATTACK");
            isAtaka = true;
            woolfAi.canMove = false;
            animWoolf.SetBool("Run", false);
            if(timerBlockingAttack > 0.4f) // если истек таймер атакуем
            {
                timerBlockingAttack = 0;
                animWoolf.SetTrigger("BiteAttack");
                MasterAudio.PlaySound("woolfAttack1");
                panda.GetComponent<PandaHealth>().SetPandaHealt(powerAttack);
            }
        }else{
            woolfAi.canMove = true;
            isAtaka = false;
            if(woolfAi.canMove)// анимация бега
            {
                animWoolf.SetBool("Run", true);
            }else
            {
                animWoolf.SetBool("Run", false);
            }
        }

        if(_PandaStatus.pandaAnimationPlay == "skill3" && !isSlowMotion) // замедляем скорость волков при применении 3 скила
        {
            isSlowMotion = true;
            SlowMotion();
        }else
        {
            if(_PandaStatus.pandaAnimationPlay != "skill3" && isSlowMotion)
            {
                isSlowMotion = false;
                SlowMotion();
            }
        }
    }


    public void AttackFromPanda(int damage) // атака сто стороны панды
    {
        if(isDeadWoolf)
            return; 

        if(woolfLife <= 0) // смерть волка
        {
            WoolfDead();
        }else{ // удар по волку
            uiHealthSlider.SetActive(true);
            uiHealthSlider.GetComponent<SetSliderBar>().SetCurrentValue(woolfLife -= damage);
            if(!woolfAi.canMove)
                animWoolf.SetTrigger("TakeDamage"); // анимация повреждения только если волк стоит
            MasterAudio.PlaySound("SwordToTarget_woolf");

            // ниже очки повреждения через pool manager
            Transform myInst = PoolManager.Pools["uiDamageScore"].Spawn(uiTextDamage.transform);
            myInst.GetComponent<UiTextDamage>().GoAnim(damage);
            myInst.transform.position = transform.position;
            PoolManager.Pools["uiDamageScore"].Despawn(myInst, 3);

            // ниже эффект VFX для ускорения через pool manager
            Transform myInstHit = PoolManager.Pools["poolVFX"].Spawn(vfxHit);
            //myInstHit.transform.position = transform.position; // приаттачиваем к панде
            myInstHit.transform.SetParent(transform);
            myInstHit.localPosition = Vector3.zero;
            PoolManager.Pools["poolVFX"].Despawn(myInstHit, 3);            
        }
    }

    void WoolfDead(){ // смерть волка
        print("Dead");
            //gameObject.layer = 0;
            //panda.GetComponent<Detector>().ReleaseDetection(gameObject);
            //GetComponent<DetectableObject>().DetectionReleased(gameObject);
            animWoolf.SetBool("Run", false);
            animWoolf.SetTrigger("Dead");            
            uiHealthSlider.SetActive(false);
            GetComponent<CharacterController>().enabled = false;
            GetComponent<EnemyWoolfAi>().radius = 0.01f;
            GetComponent<EnemyWoolfAi>().enabled = false;
            MasterAudio.PlaySound("SwordToTarget_woolf");
            MasterAudio.PlaySound("woolfDead");
            woolfAi.canMove = false;
            isDeadWoolf = true;
            panda.GetComponent<PandaHealth>().SetPandaMana(30); // добавляем ману панде
    }


    void SlowMotion() // включаем / отключаем режим slowmotion
    {
        if(isSlowMotion)
        {
            print("woolfSlow - on");
            GetComponent<Animator>().SetFloat("speedRun", 0.15f);
            woolfAi.maxSpeed = woolfAi.maxSpeed / 5.0f;

        }else
        {
            print("woolfSlow - off");
            GetComponent<Animator>().SetFloat("speedRun", 1.0f);
            woolfAi.maxSpeed = woolfAi.maxSpeed * 5.0f;
        }        
    }
}
