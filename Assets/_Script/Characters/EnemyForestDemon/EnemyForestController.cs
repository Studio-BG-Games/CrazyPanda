using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DarkTonic.MasterAudio;
using PathologicalGames;


public class EnemyForestController : MonoBehaviour
{
    public int forestfLife = 30;
    public GameObject panda;
    public float distanceToPanda; // расстояние до панды
    public float minSpeed = 14;
    public float maxSpeed = 16;
    RichAI forestAi; // ии лесной демон
    public Animator animForest; // антиматор
    bool isAtaka; // если да лесной демон атакует

    public float timerIsAtakaFromPanda; // отсчет времени когда лесному демону снова позволено атаковать (после атаки панды)
    public bool isDeadForest; // если да лесной демон умер

    private bool isSlowMotion; // если да включен режим замедления

    [Space(5)]
    public GameObject uiTextDamage;
    public GameObject VFX_Forest_fromPandaHit;
    public GameObject ForestProjectile5; // атака лесной демона камнем из под земли

    [Space(5)]
    private float timerBlockingAttack; // отсчет времени когда лесному демону снова позволено атаковать (после собственно атаки)
    public GameObject uiHealthSliderForest;
    
    public int powerAttack = -5;
    
    
    void Awake()
    {
        panda = GameObject.FindWithTag("Panda");
        
        forestAi = GetComponent<RichAI>();
        forestAi.maxSpeed = Random.Range(minSpeed, maxSpeed);
        uiHealthSliderForest.GetComponent<SetSliderBar>().SetMaxValueSlider(forestfLife);
        
    }
    
    void Update()
    {
        timerBlockingAttack += Time.deltaTime;

        if(isDeadForest)
        {
            return; // прерываем выполнение Update - лесной демон мертв
        }

        timerIsAtakaFromPanda += Time.deltaTime;
        distanceToPanda = Vector3.Distance(transform.position, panda.transform.position);
        //print(distanceToPanda);
        

        if(distanceToPanda < 15.0f && timerIsAtakaFromPanda > 0.8f)
        { // атака  
            //print("Forest TimerAtt" + timerIsAtakaFromPanda);
            isAtaka = true;
            forestAi.canMove = false;
            animForest.SetBool("run", false);
            if(timerBlockingAttack > 1.1f) // если истек таймер атакуем
            {   
                //timerIsAtakaFromPanda = 0;
                timerBlockingAttack = 0;
                animForest.SetTrigger("attack");
                
                transform.LookAt (new Vector3 (panda.transform.position.x, transform.position.y, 
                                                    panda.transform.position.z)); // поворот на цель (врашение по осям x z исключено)
                
            }
        }else
        {
            forestAi.canMove = true;
            isAtaka = false;
            if(forestAi.canMove)// анимация бега
            {
                animForest.SetBool("run", true);
            }else
            {
                animForest.SetBool("run", false);
            }
        }
    }


    public void AttackFromPanda(int damage) // атака сто стороны панды
    {
        if(isDeadForest)
            return; 

        if(forestfLife <= 0) // смерть лесной демон
        {
            ForestDead();
        }
        else
        { // удар по лесной демону
            uiHealthSliderForest.SetActive(true);
            uiHealthSliderForest.GetComponent<SetSliderBar>().SetCurrentValue(forestfLife -= damage);
            if(!forestAi.canMove)
                animForest.SetTrigger("TakeDamage"); // анимация повреждения только если лесной демон стоит
            MasterAudio.PlaySound("SwordToTarget_woolf");

            // ниже очки повреждения через pool manager
            Transform myInst = PoolManager.Pools["uiDamageScore"].Spawn(uiTextDamage.transform);
            myInst.GetComponent<UiTextDamage>().GoAnim(damage);
            myInst.transform.position = transform.position + new Vector3(0, 5.0f, 0);
            PoolManager.Pools["uiDamageScore"].Despawn(myInst, 3);

            // ниже эффект VFX для ускорения через pool manager
            Transform myInstHit = PoolManager.Pools["poolVFX"].Spawn(VFX_Forest_fromPandaHit);
            //myInstHit.transform.position = transform.position; // приаттачиваем к панде
            myInstHit.transform.SetParent(transform);
            myInstHit.localPosition = Vector3.zero;
            PoolManager.Pools["poolVFX"].Despawn(myInstHit, 3);
        }
    }

    void ForestDead()
    { // смерть лесной демона
        print("DeadForest");
        //gameObject.layer = 0;
        //panda.GetComponent<Detector>().ReleaseDetection(gameObject);
        //GetComponent<DetectableObject>().DetectionReleased(gameObject);
        animForest.SetBool("run", false);
        animForest.SetTrigger("death");            
        uiHealthSliderForest.SetActive(false);
        GetComponent<CharacterController>().enabled = false;
        GetComponent<RichAI>().radius = 0.01f;
        GetComponent<RichAI>().enabled = false;
        MasterAudio.PlaySound("SwordToTarget_woolf");
        MasterAudio.PlaySound("ForestDeath");
        forestAi.canMove = false;
        isDeadForest = true;
        panda.GetComponent<PandaHealth>().SetPandaMana(100); // добавляем ману панде
    }


    void SlowMotion() // включаем / отключаем режим slowmotion
    {
        if(isSlowMotion)
        {
            print("Slow - on");
            animForest.SetFloat("speedRun", 0.15f);
            forestAi.maxSpeed = forestAi.maxSpeed / 5.0f;

        }else
        {
            print("Slow - off");
            animForest.SetFloat("speedRun", 1.0f);
            forestAi.maxSpeed = forestAi.maxSpeed * 5.0f;
        }        
    }


    
}
