using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIPandaSettings : MonoBehaviour
{
    public GameObject UIPopUp_Eyes; // всплывающее окно
    public GameObject UIPopUp_Hats;
    public GameObject UIPopUp_Clothes;
    public GameObject UIPopUp_AccesChain;
    public GameObject UIPopUp_AccesRing;
    public GameObject UIPopUp_AccesEarring;
    [Space(5)] 
    public GameObject[] attEyes; // массив иконок которые могут отображаться в разделе атрибуты (в кнопках аттрибутов)
    public GameObject[] attHats;
    public GameObject[] attClothes;
    public GameObject[] attAccChain;
    public GameObject[] attAccRing;
    public GameObject[] attAccEaring;
    
    [Space(15)]
    public TextMeshProUGUI statTxtStrenght; // UI значения статов
    public TextMeshProUGUI statTxtLazyness;
    public TextMeshProUGUI statTxtCrazyness;
    public TextMeshProUGUI statTxtLuckiness;
    
    
    public GameObject pandaPanel;
    void Start()
    {
        LoadingAllIcons(); // загружаем сохраненные данные иконок

    }

    public void ClosePandaPanel()
    {
        pandaPanel.SetActive(false);
    }
    
    // ниже кнопки атрибутов которые открывают PopUP окна
    public void PopEyesShow()
    {
        UIPopUp_Eyes.SetActive(true);
    }
    public void PopHatsShow()
    {
        UIPopUp_Hats.SetActive(true);
    }
    public void PopClothesShow()
    {
        UIPopUp_Clothes.SetActive(true);
    }
    public void PopChainShow()
    {
        UIPopUp_AccesChain.SetActive(true);
    }
    public void PopRingShow()
    {
        UIPopUp_AccesRing.SetActive(true);
    }
    public void PopEaringShow()
    {
        UIPopUp_AccesEarring.SetActive(true);
    }
    
    // ниже кнопки в самих PopUp окнах. в int храниться номер item по которому кликнул пользователь
    public void EyesSelect(int numItem)
    {
        UIPopUp_Eyes.SetActive(false);
        UpdateIconStatus(attEyes, numItem);
        ES3.Save("eye", numItem); // сохраняем значение
    }
    public void HatsSelect(int numItem)
    {
        UIPopUp_Hats.SetActive(false);
        UpdateIconStatus(attHats, numItem);
        ES3.Save("hat", numItem);
    }
    public void ClothesSelect(int numItem)
    {
        UIPopUp_Clothes.SetActive(false);
        UpdateIconStatus(attClothes, numItem);
        ES3.Save("clothe", numItem);
    }
    public void ChainSelect(int numItem)
    {
        UIPopUp_AccesChain.SetActive(false);
        UpdateIconStatus(attAccChain, numItem);
        ES3.Save("chain", numItem);
    }
    public void RingSelect(int numItem)
    {
        UIPopUp_AccesRing.SetActive(false);
        UpdateIconStatus(attAccRing, numItem);
        ES3.Save("ring", numItem);
    }
    public void EaringSelect(int numItem)
    {
        UIPopUp_AccesEarring.SetActive(false);
        UpdateIconStatus(attAccEaring, numItem);
        ES3.Save("earring", numItem);
    }


    void UpdateIconStatus(GameObject[] massIcon, int idItem) // обновление всех иконок-атрибутов и статов
    {
        for (int i = 0; i < massIcon.Length; i++) // обновление иконок
        {
            if (i == idItem)
            {
                massIcon[i].SetActive(true);
            }
            else
            {
                massIcon[i].SetActive(false);
            }
        }
    }


    void LoadingAllIcons()
    {
        if(ES3.KeyExists("eye"))
            _SV.popEyes = ES3.Load<int>("eye");
        if(ES3.KeyExists("hat"))
            _SV.popHats = ES3.Load<int>("hat");
        if(ES3.KeyExists("clothe"))
            _SV.popClothes = ES3.Load<int>("clothe");
        if(ES3.KeyExists("chain"))
            _SV.popAccesChain = ES3.Load<int>("chain");
        if(ES3.KeyExists("ring"))
            _SV.popAccesRing = ES3.Load<int>("ring");
        if(ES3.KeyExists("earring"))
            _SV.popAccesEarring = ES3.Load<int>("earring");
        
        print(ES3.Load<int>("eye"));
        print(ES3.Load<int>("hat"));
        print(ES3.Load<int>("clothe"));
        print(ES3.Load<int>("chain"));
        print(ES3.Load<int>("ring"));
        print(ES3.Load<int>("earring"));
        
        // включаем иконки, и атоматом заполняются поля stats из серипта на самих items
        UpdateIconStatus(attEyes, _SV.popEyes);
        UpdateIconStatus(attHats, _SV.popHats);
        UpdateIconStatus(attClothes, _SV.popClothes);
        UpdateIconStatus(attAccChain, _SV.popAccesChain);
        UpdateIconStatus(attAccRing, _SV.popAccesRing);
        UpdateIconStatus(attAccEaring, _SV.popAccesEarring);
        
        
    }
    
    void Update()
    {
        statTxtStrenght.text = _SV.strenghtCurrent.ToString(); // отображаем значения статов в UI
        statTxtLazyness.text = _SV.lazynessCurrent.ToString();
        statTxtCrazyness.text = _SV.crazynessCurrent.ToString();
        statTxtLuckiness.text = _SV.luckynessCurrent.ToString();
    }
    
    
}
