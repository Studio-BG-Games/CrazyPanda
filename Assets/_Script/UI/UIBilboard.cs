using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBilboard : MonoBehaviour
{
    public Transform cam;
  
    void Awake()
    {
        cam = GameObject.FindWithTag("MainCamera").transform;
    }

    void LateUpdate() // LateUpdate чтобы сперва перемещалась камера, а затем мы поворачиваем ui элемент лицом к ней (чтоб избежать дрожжания)
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
