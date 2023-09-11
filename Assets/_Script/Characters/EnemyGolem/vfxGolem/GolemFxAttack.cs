using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// скирпт производит специальную атаку голема по панде (камень из под земли)
public class GolemFxAttack : MonoBehaviour
{
    bool isShot; // если да атака была произведена - чтоб избежать повторной атаки
    public int attackValue;
 

    void OnEnable()
    {
        isShot = false;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    void OnTriggerEnter(Collider col) 
    {
        if(col.name == "Panda" && !isShot)
        {
            isShot = true;
            col.GetComponent<PandaHealth>().SetPandaHealt(attackValue);
        }
    }
}
