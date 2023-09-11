using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIEscMenu : MonoBehaviour
{
    public GameObject gamePanel;
    public GameObject escPanel;
    public GameObject pandaPanel;
    public TMP_Dropdown dropDownMenu;
    public Toggle isFullScreen;
    
    void Start()
    {
        isFullScreen.onValueChanged.AddListener(delegate{
            SwitchFullScreen(isFullScreen);
        });
        Cursor.visible = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!escPanel.activeSelf)
            {
                escPanel.SetActive(true);
                Time.timeScale = 0.0f;
                Cursor.visible = true;
                _SV.ifPausaESC = true;
            }
            else
            {
                escPanel.SetActive(false);
                Time.timeScale = 1.0f;
                Cursor.visible = false;
                _SV.ifPausaESC = false;
            }
        }
    }

    public void PandaPanelShow()
    {
        pandaPanel.SetActive(true);
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        Time.timeScale = 1.0f;
        Cursor.visible = false;
        _SV.ifPausaESC = false;
        string scene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(scene);
    }

    public void LoadCustomeScene(int sceneNumber)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(sceneNumber);
    }

    public void SwitchResolution()
    {
        if(dropDownMenu.value == 0)
        {
            Screen.SetResolution(1280, 720, isFullScreen.isOn);
            print("Full 0");
        }
        else if(dropDownMenu.value == 1)
        {
            Screen.SetResolution(1920, 1080, isFullScreen.isOn);
            print("Full 1");
        }
        else if(dropDownMenu.value == 2)
        {
            Screen.SetResolution(2560, 1440, isFullScreen.isOn);
            print("Full 2");
        }
        else if(dropDownMenu.value == 3)
        {
            Screen.SetResolution(3840, 2160, isFullScreen.isOn);
        }
    }


    public void SwitchFullScreen(Toggle toggleValue)
    {
        if(toggleValue.isOn)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            print("full scr");
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            print("win scr");
        }
    }
}
