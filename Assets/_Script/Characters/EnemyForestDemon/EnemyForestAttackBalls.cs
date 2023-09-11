using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;
using DarkTonic.MasterAudio;

public class EnemyForestAttackBalls : MonoBehaviour
{
    public GameObject ForestProjectile5;
    public GameObject VFXForestHandBefor;
    public Transform positionStartBall;
    private Transform panda;

    void Awake()
    {
        
    }
    
    void Start()
    {
        panda = transform.parent.GetComponent<EnemyForestController>().panda.transform;
    }

    
    void Update()
    {
        
    }

    public void AttackBalls() // атака шарами
    {
        MasterAudio.PlaySound("ForestBallFly", 0.2f);
        MasterAudio.PlaySound("ForestBallAttack", 0.05f);
        //print("PowerAttack_Forest");
        // ниже эффект VFX для RMB через pool manager
        Transform myInstForest = PoolManager.Pools["poolVFX"].Spawn(ForestProjectile5);
        myInstForest.transform.position = positionStartBall.position;
        myInstForest.LookAt(panda.transform.position  + new Vector3(0, 0.8f, 0));

        PoolManager.Pools["poolVFX"].Despawn(myInstForest, 6);
        VFXForestHandBefor.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    }

    public void VFXForestHandScale()
    {
        VFXForestHandBefor.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
    }
}
