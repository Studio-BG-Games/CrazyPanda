using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[HelpURL("http://arongranberg.com/astar/docs/class_partial1_1_1_astar_a_i.php")] // ссылка на описание скрипта
public class EnemyWoolf : MonoBehaviour
{
    public Transform targetPosition; // цель к которой будем перемещаться
    private CharacterController woolfController;

    public Path path;
    public float speed = 2;
    public float nextWaypointDistance = 3;
    private int currentWaypoint = 0;
    public bool isFinishWalk; // если да мы приблизились к объекту
    Seeker seeker;

    float timerResearchPath;

    void Start()
    {
        seeker = GetComponent<Seeker>(); // ссылка на компноент который отвечает за поиск цели
        woolfController = GetComponent<CharacterController>();

        seeker.StartPath(transform.position, targetPosition.position, OnPathComplete); // ставим задачу поиска пути до цели
        
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("Мы получили путь. Есть ли ошибки? " + p.error);
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void Update()
    {
        timerResearchPath += Time.deltaTime;
        if(timerResearchPath > 0.5f) 
        {
            timerResearchPath = 0;
            seeker.StartPath(transform.position, targetPosition.position, OnPathComplete); // ставим задачу поиска пути до цели каждве пол секундыы
        }

        if(path == null) // если путь отсутствует прерываем функцию Update
        {
            return;
        }

        float distanceToWaypoint; // расстояние до след путевой точки

        while(true)
        {
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);

            if(distanceToWaypoint < nextWaypointDistance)
            {
                if(currentWaypoint + 1 < path.vectorPath.Count)
                {
                    currentWaypoint ++;
                    //Vector3 dir = (transform.position, path.).
                    //RotationCharacter()
                }else 
                {
                    isFinishWalk = true; // мы достигли цели
                    break;
                }
            }else
            {
                break;
            }
        }

        var speedFactor = isFinishWalk ? Mathf.Sqrt(distanceToWaypoint/nextWaypointDistance) : 1f; // плавно снижаем скорость перед остановкой
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized; // определяем нормализованное(1) направление кудв нужно двигаться
        Vector3 velocity = dir * speed * speedFactor; // умножаем направление на скорость чтоб получить нужную скорость для движения

        woolfController.SimpleMove(velocity); // перемещение


    }

    void RotationCharacter(Vector3 dir) // вращение персонажа
    {
            
    }

}
