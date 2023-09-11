using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAttributesPropertySet : MonoBehaviour
{

    public int strenght;
    public int lazyness;
    public int crazyness;
    public int luckyness;

    private void OnEnable()
    {
        _SV.strenghtCurrent += strenght;
        _SV.lazynessCurrent += lazyness;
        _SV.crazynessCurrent += crazyness;
        _SV.luckynessCurrent += luckyness;
    }
    
    private void OnDisable()
    {
        _SV.strenghtCurrent -= strenght;
        _SV.lazynessCurrent -= lazyness;
        _SV.crazynessCurrent -= crazyness;
        _SV.luckynessCurrent -= luckyness;
    }

    
}
