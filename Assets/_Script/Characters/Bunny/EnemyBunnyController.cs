using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using PathologicalGames;
using DarkTonic.MasterAudio;

public class EnemyBunnyController : MonoBehaviour
{
    public int bunnyLife = 30;
    public float distanceToPanda; // расстояние до панды
    public float minSpeed = 10;
    public float maxSpeed = 14;
    public Animator animBunny; // антиматор
    private GameObject panda;
    public bool isDeadBunny; // если да bunny мертв
    public GameObject fX_BunnyAttack;
    public int powerAttack = -5;
    private RichAI bunnyAi;

    bool isAtaka; // если да bunny атакует
    public float timerIsAtakaFromPanda; // отсчет времени когда волку снова позволено атаковать
    private float timerBlockingAttack;
    private bool isSlowMotion; // если да включен режим замедления
    
    [Space(5)]
    public GameObject uiTextDamage;
    public GameObject uiHealthSlider;

    [Space(5)]
    [Header("PUSH")]
    [SerializeField]
    private CharacterController bunnyControll;
    public Vector3 directionPush;
    public float pushTime; // продолжительность полета от толчка
    public AnimationCurve curvePushSpeed; // кривая для получения скорости
    private float pushSpeed;
    private bool isPushed; // если да идет анимация толчка (своя гравитация не RichAi)

    [Space(5)]
    [Header("GRAVITY")]
    public Transform checkGroung; // объект проверяет скорость падения
    public float groundDistance  = 0.4f; // если меньще этого расстояния, то стоим на земле
    public LayerMask groundMask; // выбираем с какими слоями работает проверка на столкновение с землей
    private bool isGrounded; // если да мы стоим на земле
    private Vector3 velocity; // скорость падения
    public float gravity = -9.8f; // скорость падения ставим здесь


    
    void Awake()
    {
        panda = GameObject.FindWithTag("Panda");
        bunnyAi = GetComponent<RichAI>();
        bunnyAi.maxSpeed = Random.Range(minSpeed, maxSpeed);
        uiHealthSlider.GetComponent<SetSliderBar>().SetMaxValueSlider(bunnyLife);
    }    

    void Start()
    {
        directionPush = new Vector3(0, 1.0f, 0.4f);
    }

    void Update()
    {
        timerBlockingAttack += Time.deltaTime;
         if(isDeadBunny)
        {
            return; // прерываем выполнение Update - волк мертв
        }

        timerIsAtakaFromPanda += Time.deltaTime;
        distanceToPanda = Vector3.Distance(transform.position, panda.transform.position);
        //print(distanceToPanda);
        

        if(distanceToPanda < 2.3f && timerIsAtakaFromPanda > 0.8f){ // атака  woolfAi.remainingDistance
        //print("WATTACK");
            isAtaka = true;
            bunnyAi.canMove = false;
            animBunny.SetBool("Dash", false);
            if(timerBlockingAttack > 0.4f) // если истек таймер атакуем
            {
                timerBlockingAttack = 0;
                animBunny.SetTrigger("BiteAttack");
                MasterAudio.PlaySound("BunnyAttack1");
                panda.GetComponent<PandaHealth>().SetPandaHealt(powerAttack);
            }
        }else{
            bunnyAi.canMove = true;
            isAtaka = false;
            if(bunnyAi.canMove)// анимация бега
            {
                animBunny.SetBool("Dash", true);
            }else
            {
                animBunny.SetBool("Dash", false);
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
    # region PUSH
    public void OnTriggerEnter(Collider col) 
    {
        if (col.name == "pandaFreeSword Variant" && (_PandaStatus.pandaAnimationPlay == "attack_LMouse1"
            || _PandaStatus.pandaAnimationPlay == "attack_RMouse1"
            || _PandaStatus.pandaAnimationPlay == "skill1"
            || _PandaStatus.pandaAnimationPlay == "skill2"
            || _PandaStatus.pandaAnimationPlay == "skill3")) 
        { // удар от панды
            print("bunny push");
            StartCoroutine(PushCoroutine());
        }
     }


    public IEnumerator PushCoroutine() // рывок при помощи 
    {
        
        float startTime = Time.time + pushTime;
        float timeForCurve = 0;
        while(startTime > Time.time) // толчок
        {
            Gravity();
            //transform.LookAt(targetPosition, Vector3.up); // поворот на цель
            timeForCurve += Time.deltaTime; //время для кривой возвращающей скорость
            pushSpeed = curvePushSpeed.Evaluate(timeForCurve); // получаем скорость из кривой
            Vector3 direct = -transform.forward + directionPush;
            bunnyControll.Move(direct * pushSpeed * Time.deltaTime);

            bunnyAi.enabled = false;
            yield return null;
        }
        bunnyAi.enabled = true;
        //print("EndPush");
    }

    void Gravity() // гравитация вместо RichAi (включаем когда RichAI выключен)
    {
        // определяем стоим ли на земле и реализуем гравитацию
        isGrounded = Physics.CheckSphere(checkGroung.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0) 
        {
            velocity.y = -2;
        }
        velocity.y += gravity *Time.deltaTime; // вычисляем скорость падения
        bunnyControll.Move(velocity * Time.deltaTime); // анимация падения
    }
    #endregion PUSH

    public void AttackFromPanda(int damage) // атака сто стороны панды
    {
        if(isDeadBunny)
            return; 

        if(bunnyLife <= 0) // смерть волка
        {
            BunnyDead();
        }else{ // удар по волку
            uiHealthSlider.SetActive(true);
            uiHealthSlider.GetComponent<SetSliderBar>().SetCurrentValue(bunnyLife -= damage);
            
            MasterAudio.PlaySound("SwordToTarget_woolf");

            // ниже очки повреждения через pool manager
            Transform myInst = PoolManager.Pools["uiDamageScore"].Spawn(uiTextDamage.transform);
            myInst.GetComponent<UiTextDamage>().GoAnim(damage);
            myInst.transform.position = transform.position;
            PoolManager.Pools["uiDamageScore"].Despawn(myInst, 3);

            // ниже эффект VFX для ускорения через pool manager
            Transform myInstHit = PoolManager.Pools["poolVFX"].Spawn(fX_BunnyAttack);
            //myInstHit.transform.position = transform.position; // приаттачиваем к панде
            myInstHit.transform.SetParent(transform);
            myInstHit.localPosition = Vector3.zero;
            PoolManager.Pools["poolVFX"].Despawn(myInstHit, 3);
        }
    }

    void BunnyDead(){ // смерть волка
        print("Dead");
            //gameObject.layer = 0;
            //panda.GetComponent<Detector>().ReleaseDetection(gameObject);
            //GetComponent<DetectableObject>().DetectionReleased(gameObject);
            animBunny.SetBool("Dash", false);
            animBunny.SetTrigger("Die");            
            uiHealthSlider.SetActive(false);
            bunnyControll.enabled = false;
            bunnyAi.radius = 0.01f;
            bunnyAi.enabled = false;
            MasterAudio.PlaySound("SwordToTarget_woolf");
            MasterAudio.PlaySound("BunnyDie");
            bunnyAi.canMove = false;
            isDeadBunny = true;
            panda.GetComponent<PandaHealth>().SetPandaMana(5); // добавляем ману панде
    }

    void SlowMotion() // включаем / отключаем режим slowmotion
    {
        if(isSlowMotion)
        {
            print("woolfSlow - on");
            animBunny.SetFloat("speedRun", 0.15f);
            bunnyAi.maxSpeed = bunnyAi.maxSpeed / 5.0f;

        }else
        {
            print("woolfSlow - off");
            animBunny.SetFloat("speedRun", 1.0f);
            bunnyAi.maxSpeed = bunnyAi.maxSpeed * 5.0f;
        }        
    }
}
