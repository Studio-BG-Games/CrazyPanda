using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PandaHealth : MonoBehaviour
{
    [Header("Health")]
    public int _maxHealthPanda = 100;
    public int _currentHealthPanda;
    public TextMeshProUGUI _textHealth;
    public SetSliderBar _healthBar;

    [Space(5)]
    [Header("Mana")]
    public int _maxManaPanda = 100;
    public int _currentManaPanda;
    //public TextMeshProUGUI _textMana;
    public SetSliderBar _manaBar;

    


    bool isPandaDead; // если да панда убита

    
    void Start()
    {
        _currentHealthPanda = _maxHealthPanda;
        _healthBar.SetMaxValueSlider(_maxHealthPanda);
        //_textHealth.text = _currentHealthPanda.ToString() +  "/" + _maxHealthPanda.ToString();

        _currentManaPanda = _maxManaPanda;
        _manaBar.SetMaxValueSlider(_maxManaPanda);
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SetPandaHealt(_maxHealthPanda);
            SetPandaMana(_maxManaPanda);
        }
    }

    public void SetPandaHealt(int addHealth) // прибавляем или отнимаем жизни
    {   
        if(isPandaDead)
        {
            return;
        }
        _currentHealthPanda += addHealth;
        _healthBar.SetCurrentValue(_currentHealthPanda);
        //_textHealth.text = _currentHealthPanda.ToString() +  "/" + _maxHealthPanda.ToString();

        if(_currentHealthPanda > _maxHealthPanda)
            _currentHealthPanda = _maxHealthPanda;

        if(_currentHealthPanda < 0)
            _currentHealthPanda = 0;

        if(_currentHealthPanda <= 0)
        {
            DeadPanda();
        }        
    }

    public void SetPandaMana(int addMana) // прибавляем или отнимаем ману
    {
        if(isPandaDead)
        {
            return;
        }
        _currentManaPanda += addMana; // меняем значение маны
        _manaBar.SetCurrentValue(_currentManaPanda);

        if(_currentManaPanda > _maxManaPanda)
            _currentManaPanda = _maxManaPanda;
        
        if(_currentManaPanda < 0)
            _currentManaPanda = 0;

    }

    void DeadPanda()
    {
        print("DeadPanda");
        GetComponent<CharacterMovementControl>().animPanda.SetTrigger("dead");
        GetComponent<CharacterController>().enabled = false;
        GetComponent<CharacterMovementControl>().enabled = false;
        isPandaDead = true;
    }
}
