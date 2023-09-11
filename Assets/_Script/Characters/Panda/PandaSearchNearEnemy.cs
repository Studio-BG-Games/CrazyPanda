using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandaSearchNearEnemy : MonoBehaviour
{   
    //*** Цель скрипта найти блоижайшего врага и развернуть героя к нему

    //public GameObject ty; // Player
	public AnimationCurve curve; // for Collider animation
	//public GameObject particleFight;

    public GameObject enemySearch; // объект коллайдер который будет увеличиваться и искать столкновения с врагом
	float timerForCurve = 0;
	public bool noRepeateSearch;
    //public Transform positionEnemy; // здесь храниться позиция найденного противника


	
	void Update () 
    {
        if(noRepeateSearch){ 
			//Debug.Log("noRepeateFight");
			float scaleValue; // var for scale
			scaleValue = curve.Evaluate(timerForCurve); //считываем значение из кривой

			timerForCurve += Time.deltaTime;
			enemySearch.transform.localScale = new Vector3(scaleValue,scaleValue,scaleValue); // scale collider

			if(scaleValue >= 35.0f){
            	   	//Debug.Log("DontFight");
					noRepeateSearch = false;
					timerForCurve = 0; //reset timer	
					enemySearch.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); // scale reset			
			}
		}
    }


    void OnTriggerEnter(Collider col) 
    {
		if(col.tag == "enemy_woolf") // atack zombie / атакуем стандартного зомби 
        {
            print("search WOOLF Golem!");
            noRepeateSearch = false;
            timerForCurve = 0; //reset timer
            //transform.parent.gameObject.transform.rotation = Quaternion.LookRotation(-col.gameObject.transform.forward);// вращение панды на противника
            transform.parent.LookAt(col.gameObject.transform);

            enemySearch.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); // scale reset
        }
    }






}
