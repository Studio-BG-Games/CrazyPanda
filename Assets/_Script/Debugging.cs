using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Debugging : MonoBehaviour
{
    public GameObject panelDebug;
    public TextMeshProUGUI textFps;
    float timerFps;
	bool showPanel;
	float timerActivation;
	int countClick; // кол-во кликов
    

    public TextMeshProUGUI pandaStatusAttack;
    public TextMeshProUGUI golemStatusAttack;
    
    void Start()
    {
        Application.targetFrameRate = 60;
    }
    

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Keypad0)){Time.timeScale = 1.0f;}
        if(Input.GetKeyDown(KeyCode.Keypad1)){Time.timeScale = 0.1f;}
        if(Input.GetKeyDown(KeyCode.Keypad2)){Time.timeScale = 0.2f;}
        if(Input.GetKeyDown(KeyCode.Keypad3)){Time.timeScale = 0.3f;}
        if(Input.GetKeyDown(KeyCode.Keypad5)){Time.timeScale = 0.5f;}

        timerActivation += Time.deltaTime;
		
        if(Input.GetKeyDown(KeyCode.K)) // быстрое тройное нажатие
        {
            if(timerActivation > 0.5f) // просрочили все обнуляем
            {
                countClick = 1;
                timerActivation = 0;
            }else{ // успели нажать добавляем значение
                countClick += 1;
                timerActivation = 0;
            }

            if(countClick >=3){
                ShowHideDebug();
                countClick = 0;
            }
        }


		if(showPanel){ // обновляем переменные если UI дебага показана
			UpdateVariable();
		}

        
        timerFps += Time.deltaTime;
        if(timerFps > 0.05f)
        {
            float fps = (1 / Time.unscaledDeltaTime);
            int fpsRound = (int)fps;
            textFps.text = "" + fpsRound;
            timerFps = 0;
        }
    }

    public void ShowHideDebug(){ // on/off panel

		if(showPanel){ // показать спрятать панель
			panelDebug.SetActive(false);
            showPanel = false;
            //print("ShowDebug");
		}else{
			panelDebug.SetActive(true);
            showPanel = true;
            //print("HideDebug");
		}
	}

    void UpdateVariable() // обновляем переменные
    {  
        pandaStatusAttack.text = "pandaStatusAttack = " + _PandaStatus.pandaAnimationPlay;
        golemStatusAttack.text = "golemStatusAttack = " + _GolemStatus.golemAnimationPlay;
    }

    
}
