using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;
using DarkTonic.MasterAudio;

public class GolemPowerAttack : MonoBehaviour
{
    public GameObject FX_GolemAttack;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void PowerAttackGolem()
    {
        print("PA Golem");
        // ниже эффект VFX для RMB через pool manager
        Transform myInstPowerAttack = PoolManager.Pools["poolVFX"].Spawn(FX_GolemAttack);
        myInstPowerAttack.transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z) + transform.forward * 1.7f;
        //myInstPowerAttack.rotation = transform.rotation;

        PoolManager.Pools["poolVFX"].Despawn(myInstPowerAttack, 4);

        MasterAudio.PlaySound("GolemAttackStone");
    }
}
