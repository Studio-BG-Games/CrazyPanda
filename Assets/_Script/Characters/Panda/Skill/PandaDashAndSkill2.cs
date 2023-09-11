using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 
 public class PandaDashAndSkill2 : MonoBehaviour
 {
    public Detector _detector; // скипт ищущий вргов и добавляющий их в List
    public AnimationCurve dashCurveEasing; // кривая отвечающая за скорость равка
    public Transform targetPosition; // цель определяется методом SearchNearEnemy
    float distanceToTarget;

    public float dashTime; // продолжительность рывка
    public float dashSpeed;
    bool isDash; // если да выполняется рывок

    CharacterMovementControl _charControll;

    Vector3 ddaash;

    void Start() 
    {
        _charControll = GetComponent<CharacterMovementControl>();
        _detector = GetComponent<Detector>();
    }

    public void StartDash(bool isSkill2)
    {        
        StartCoroutine(MoveCoroutine(isSkill2));
    }

    public IEnumerator MoveCoroutine(bool isSkill2) // рывок при помощи 
    {
        float startTime = Time.time + dashTime;
        float timeForCurve = 0;
        while(startTime > Time.time) // рывок
        {
            if(isSkill2) // если да поворачиваемся на врага и потом происходит рывок
            {
                SearchNearEnemy(); // находим ближайшего противника
                if(targetPosition != null) // чтоб не было ошибки если в списке нет врагов и трансформ пуст
                {
                    transform.LookAt (new Vector3 (targetPosition.position.x, transform.position.y, 
                                                    targetPosition.position.z)); // поворот на цель (врашение по осям x z исключено)
                    transform.position = transform.position;            
                }
            }

            //transform.LookAt(targetPosition, Vector3.up); // поворот на цель
            timeForCurve += Time.deltaTime; //время для кривой возвращающей скорость
            dashSpeed = dashCurveEasing.Evaluate(timeForCurve); // получаем скорость из кривой
            _charControll.charControll.Move(transform.forward * dashSpeed * Time.deltaTime);
            yield return null;
        }
    }


    void SearchNearEnemy() // поиск ближайшего противника
    {
        float distance = Mathf.Infinity; // присваиваем бесконечность
        Vector3 position = transform.position; // наша позиция
        foreach(GameObject go in _detector.detectedObjects)
        {   
            if(go.GetComponent<EnemyWoolfController>() != null)
            {
                if(!go.GetComponent<EnemyWoolfController>().isDeadWoolf) // атака по волку
                {
                    float distanceCurrent = Vector3.Distance(go.transform.position, position); // расстояние до объекта
                    if(distanceCurrent < distance) // если не выполняется остается объект который при проверке был превым
                    {
                        targetPosition = go.transform;
                        distance = distanceCurrent;
                        print("NearJbject = " + go.name);
                    }
                }
            }else if(go.GetComponent<EnemyGolemController>() != null)
            {
                 if(!go.GetComponent<EnemyGolemController>().isDeadGolem) // атака по волку
                {
                    float distanceCurrent = Vector3.Distance(go.transform.position, position); // расстояние до объекта
                    if(distanceCurrent < distance) // если не выполняется остается объект который при проверке был превым
                    {
                        targetPosition = go.transform;
                        distance = distanceCurrent;
                        print("NearJbject = " + go.name);
                    }
                }
            }
        }
        
    }


}