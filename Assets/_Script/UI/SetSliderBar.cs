using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSliderBar : MonoBehaviour
{
    public Slider _slider;

    void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    public void SetMaxValueSlider(int maxValue)
    {
        _slider.maxValue = maxValue;
        _slider.value = maxValue;
    }

    public void SetCurrentValue(int setValue)
    {
        _slider.value = setValue;

    }
}
