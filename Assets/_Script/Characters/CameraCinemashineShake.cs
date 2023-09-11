using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class CameraCinemashineShake : MonoBehaviour
{
    // сласс для вызова shake эффекта для камеры cinamachine
    public UnityEvent shakeGo;
    


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            StartShake();
        }

    }

    public void ShakeStartForScript()
    {
        StartShake();
    }

    public void StartShake(){ // для старта запускаем этот метод
        shakeGo.Invoke();
        print("myEvent");

    }
}
