using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// включаем отключаем разные эффекты в зависимости от статуса анимации голема
public class GolemVfxActivate : MonoBehaviour
{
    public GameObject attack1Particles; // простая атака
    
    

    void Start()
    {
        
    }

    
    void Update()
    {
        if(_GolemStatus.golemAnimationPlay == "attack1")
        {
            attack1Particles.SetActive(true);
        }else
        {
            attack1Particles.SetActive(false);
        }
    }

}
