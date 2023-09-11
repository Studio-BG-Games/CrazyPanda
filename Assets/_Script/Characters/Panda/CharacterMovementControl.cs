using System.Collections;
using UnityEngine;
using DarkTonic.MasterAudio;
using PathologicalGames;
using Cinemachine;

public class CharacterMovementControl : MonoBehaviour
{
    public CharacterController charControll;
    public PandaAttackController _pAttackController; // скрипт выполняющий атаку наносит повреждения противникам
    public PandaSearchNearEnemy searchEnemyForTurn;
    private PandaHealth healthManaUI;
    public float speedMovement = 5.0f; // скорость ходьбы
    public float speedRun = 9.0f; // скорость бега
    public float timeSmoothTurn = 0.1f; // скорость поворота
    public float jumpHeight = 3.0f;
    private float turnSmoothVelocity;

    private float horizontal;
    private float vertical;
    public Vector3 directionMov; // направлени е движения

    // ниже переменные для симуляции гравитации
    public Transform checkGroung; // объект проверяет скорость падения
    public float groundDistance  = 0.4f; // если меньще этого расстояния, то стоим на земле
    public LayerMask groundMask; // выбираем с какими слоями работает проверка на столкновение с землей
    public float speed = 12.0f;
    public float gravity = -9.8f; // скорость падения ставим здесь
    private Vector3 velocity; // скорость падения

    private bool isGrounded; // если да мы стоим на земле

    public Animator animPanda; // контроллер анимаций
    public Transform Panda;

    public float timerBlockingAttackL; // таймер начинает отсчитывать время от 0 с момента атаки левого клика (для того чтоб можно было ограничить кол-во атак - кликов)
    public float timerBlockingAttackR; // таймер начинает отсчитывать время от 0 с момента атаки правого клика (для того чтоб можно было ограничить кол-во атак - кликов)
    public float timerBlockingSkill; // таймер начинает отсчитывать время от 0 с момента атаки скиллом (для того чтоб можно было ограничить кол-во атак - кликов)

    private bool isActionStop; // если да перемещение персонажем запрещено (выполняется атака)
    private Coroutine PausaAttackRoutin; // коротина для приостановки перемещения персонажа

    [Space(5)]
    [Header("VFX")]
    public GameObject vfxRMB;
    public GameObject vfxDash; // эффект для рывка
    public GameObject vfxSkill3; // эффект для рывка

    [Space(5)]
    public float durationAtackLMB; // продолжительность атаки левой кнопки мыши

    void Awake()
    {
        //timerBlockingSkill = 0.1f; // чтоб сразу после запуска игры можно было атаковать
        animPanda = GetComponentInChildren<Animator>();
        healthManaUI = GetComponent<PandaHealth>();
    }

    
    void Update()
    {
        Movement(); // // бег и ходьба
        JumpAndGravity(); // прыжок и гравитация

        Panda.localPosition =  new Vector3(0,0,0);

        timerBlockingAttackL += Time.deltaTime; // таймер левый клик
        timerBlockingAttackR += Time.deltaTime; // таймер правый клик
        timerBlockingSkill -= Time.deltaTime; // Таймер для скилов
        
        if(Input.GetMouseButtonDown(0) && !_SV.ifPausaESC) // LMB
        {
            Debug.LogError("--------");
            if (_pAttackController != null)
            {
                _pAttackController.noDoubleAttackLMB = false;
            }
            
            AttackMouseL();
        }

        else if(Input.GetMouseButtonDown(1) && !_SV.ifPausaESC) // RMB
        {
            Debug.LogError("--------");
            _pAttackController.noDoubleAttackRMB = false;
            AttackMouseR();            
        }

        else if(Input.GetKeyDown(KeyCode.Alpha1) && !_SV.ifPausaESC)
        {
            Debug.LogError("--------");
            Skill1();            
        }

        else if(Input.GetKeyDown(KeyCode.Alpha2) && !_SV.ifPausaESC)
        {
            Debug.LogError("--------");
            Skill2();
        }

        else if(Input.GetKeyDown(KeyCode.Alpha3) && !_SV.ifPausaESC)
        {        
            Debug.LogError("--------");
            Skill3();
        }

        else if(Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(2))
        {
            Debug.LogError("--------");
            if (!_SV.ifPausaESC)
            {
                _PandaStatus.pandaAnimationPlay = "dash";
                Dash();
            }
        }
    }

    void ChangeAnimationStatus() // обновляем инфо статус текущей атаки
    {
        AnimatorClipInfo [ ] m_CurrentClipInfo;
        m_CurrentClipInfo = animPanda.GetCurrentAnimatorClipInfo (0);
        _PandaStatus.pandaAnimationPlay = m_CurrentClipInfo[0].clip.name;
    }

    void Movement() // бег и ходьба
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        directionMov = new Vector3(horizontal, 0f, vertical).normalized; // normalized чтоб по диагонали двигался с такой же скоростью

        if(directionMov.sqrMagnitude == 0) // СТОИТ IDLE
        {
            animPanda.SetFloat("speed", 0, 0.1f,Time.deltaTime); // 
        }
        if(directionMov.sqrMagnitude > 0.1f && !Input.GetKey(KeyCode.LeftShift) && !isActionStop) // ХОДЬБА >
        {
            animPanda.SetFloat("speed", 0.5f, 0.1f,Time.deltaTime);
            //print("WALK");
            float angleTarget = Mathf.Atan2(directionMov.x, directionMov.z) * Mathf.Rad2Deg; // находим угол на который нужно повернуть персонажа
            float angleSmooth = Mathf.SmoothDampAngle(transform.eulerAngles.y, angleTarget, ref turnSmoothVelocity, timeSmoothTurn); // сглаживаем поворот персонажа

            transform.rotation = Quaternion.Euler(0, angleSmooth, 0); // поворот персонажа в ту сторону куда он идет
            charControll.Move(directionMov * speedMovement * Time.deltaTime);
        }

        if(directionMov.magnitude > 0.1f && Input.GetKey(KeyCode.LeftShift) && !isActionStop) // БЕГ >>>
        {
            animPanda.SetFloat("speed", 1.0f, 0.1f,Time.deltaTime);
            //print("RUN");
            float angleTarget = Mathf.Atan2(directionMov.x, directionMov.z) * Mathf.Rad2Deg; // находим угол на который нужно повернуть персонажа
            float angleSmooth = Mathf.SmoothDampAngle(transform.eulerAngles.y, angleTarget, ref turnSmoothVelocity, timeSmoothTurn); // сглаживаем поворот персонажа

            transform.rotation = Quaternion.Euler(0, angleSmooth, 0); // поворот персонажа в ту сторону куда он идет
            charControll?.Move(directionMov * speedRun * Time.deltaTime);
        }
    }
    
    void JumpAndGravity()
    {
        // определяем стоим ли на земле и реализуем гравитацию
        if (checkGroung == null) return;
        
        isGrounded = Physics.CheckSphere(checkGroung.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }

        velocity.y += gravity * Time.deltaTime; // вычисляем скорость падения
        charControll?.Move(velocity * Time.deltaTime); // анимация падения

        // добавляем прыжок
        if (Input.GetButtonDown("Jump") && timerBlockingAttackR > 1.0f)
        {
            timerBlockingAttackR = 0;
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
    }

    

    void AttackMouseL()
    {
        _PandaStatus.pandaAnimationPlay = "attack_LMouse1";
        
        if(timerBlockingAttackL > 0.3f && !isActionStop) // разрешаем атаку по истечению таймера
        {
            if(searchEnemyForTurn != null)
            {
                searchEnemyForTurn.noRepeateSearch = true; // поиск ближайшего врага и разворот персонажа в его сторону
            }
            
            //print(animPanda.GetCurrentAnimatorStateInfo(0).length);
            timerBlockingAttackL = 0;
            animPanda.SetTrigger("attack_LMouse1");
            MasterAudio.PlaySound("sword"); // sound
            _pAttackController.AttackLeftBtn(); // вызов метода атаки (столкновения с коллайдером и повреждения врагам итд)
        }
    }

    void AttackMouseR()
    {
        _PandaStatus.pandaAnimationPlay = "attack_RMouse1";
        
        if(timerBlockingAttackR > 1.0f) // разрешаем атаку по истечению таймера
        {
            searchEnemyForTurn.noRepeateSearch = true; // поиск ближайшего врага и разворот персонажа в его сторону
            timerBlockingAttackR = 0;
            animPanda.SetTrigger("attack_RMouse1");
            MasterAudio.PlaySound("Sword_RMB", 1, null, 0.5f); // sound
            MasterAudio.PlaySound("pandaAttack1", 1.0f, null, 0.52f);
            _pAttackController.AttackRightBtn(); // вызов метода атаки (столкновения с коллайдером и повреждения врагам итд)
            
            // ниже эффект VFX для RMB через pool manager
            Transform myInstRMB = PoolManager.Pools["poolVFX"].Spawn(vfxRMB);
            myInstRMB.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z) + transform.forward * 1.7f;
            myInstRMB.rotation = transform.rotation;
            
            PoolManager.Pools["poolVFX"].Despawn(myInstRMB, 4);

            if(PausaAttackRoutin != null)
            {
                StopCoroutine(PausaAttackRoutin);
                PausaAttackRoutin = null;
            }

            PausaAttackRoutin = StartCoroutine(PausaAttack(1.0f, true)); // запрещаем атаку(sec) и перемещение
        }    
    }
    
    void Skill1()
    {
        if(timerBlockingSkill < 0.0f) // разрешаем атаку по истечению таймера
        {
            _PandaStatus.pandaAnimationPlay = "skill1";
            animPanda.SetTrigger("skill1");
            timerBlockingSkill = 2.15f;
            MasterAudio.PlaySound("PandaVoiceSkill1");
            healthManaUI.SetPandaMana(-20); // mana
        }
        if(PausaAttackRoutin != null)
        {
            StopCoroutine(PausaAttackRoutin);
            PausaAttackRoutin = null;
        }
        PausaAttackRoutin = StartCoroutine(PausaAttack(2.8f, false)); // запрещаем атаку(sec) и перемещение        
    }

    void Skill2()
    {
        if(timerBlockingSkill < 0.0f) // разрешаем атаку по истечению таймера
        {
            _PandaStatus.pandaAnimationPlay = "skill2";
            GetComponent<PandaDashAndSkill2>().StartDash(true);
            animPanda.SetTrigger("skill2");
            timerBlockingSkill = 3.0f;
            healthManaUI.SetPandaMana(-20); // mana
           
            if(PausaAttackRoutin != null)
            {
                StopCoroutine(PausaAttackRoutin);
                PausaAttackRoutin = null;
            }
            PausaAttackRoutin = StartCoroutine(PausaAttack(3.0f, true)); // запрещаем атаку(sec) и перемещение
        }
    }

    void Skill3()
    {
        if(timerBlockingSkill < 0.0f) // разрешаем атаку по истечению таймера
        {
            //print("3_in");
            _PandaStatus.pandaAnimationPlay = "skill3";
            animPanda.SetTrigger("skill3");
            timerBlockingSkill = 3f;

            // ниже эффект VFX для 3 скила через pool manager
            /*Transform myInst = PoolManager.Pools["poolVFX"].Spawn(vfxSkill3);
            myInst.transform.position = transform.position; 
            myInst.rotation = transform.rotation;
            //myInst.transform.SetParent(transform); // приаттачиваем к панде
            PoolManager.Pools["poolVFX"].Despawn(myInst, 12);*/
            
            healthManaUI?.SetPandaMana(-30); // mana
            MasterAudio.PlaySound("skill3_rock_smashable", 1.0f, null, 2.15f);

            // дрожжание камеры cinemachine shake
            GetComponent<CinemachineImpulseSource>().GenerateImpulse();
           
            if(PausaAttackRoutin != null)
            {
                StopCoroutine(PausaAttackRoutin);
                PausaAttackRoutin = null;
            }
            PausaAttackRoutin = StartCoroutine(PausaAttack(3.1f, true)); // запрещаем атаку(sec) и перемещение
        }
    }

    void Dash()
    {
        GetComponent<PandaDashAndSkill2>().StartDash(false);
        MasterAudio.PlaySound("pandaJump1");
        
        animPanda.SetTrigger("dash");
        MasterAudio.PlaySound("Dash1");

        // ниже эффект VFX для ускорения через pool manager
        Transform myInst = PoolManager.Pools["poolVFX"].Spawn(vfxDash);
        
        myInst.transform.position = transform.position; // приаттачиваем к панде
        myInst.transform.SetParent(transform);
        PoolManager.Pools["poolVFX"].Despawn(myInst, 3);
    }

    IEnumerator PausaAttack(float timeStop, bool isMoveStopped) // пауза, пока не завершаться другие атаки панды новая не запустятся. isMove - можно ли персонажу двигаться
    {
        isActionStop = isMoveStopped;
        yield return new WaitForSeconds(timeStop);
        isActionStop = false; // разрешаем перемещаться
        PausaAttackRoutin = null;
        _PandaStatus.pandaAnimationPlay = "idle";
    }
}
