using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DarkTonic.MasterAudio;

public class GolemGo : MonoBehaviour // активируем голема
{
    public RichAI aiGolem;
    public EnemyGolemController controllGolem;

    
    public void GolrmGo()
    {
        StartCoroutine(GolemHunterGo());
    }

    IEnumerator GolemHunterGo()
    {
        yield return new WaitForSeconds(2.0f);
        aiGolem.enabled = true;
        controllGolem.enabled = true;
    }


    public void FootstepGolem()
    {
        MasterAudio.PlaySound3DFollowTransform("FootStep1",transform);
    }
}
