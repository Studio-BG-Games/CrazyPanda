using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// камера миникарты следует за пандой

public class MiniMapCamera : MonoBehaviour
{
    public Transform targetFollow;

    void Start()
    {

    }


    void Update()
    {
        transform.position = new Vector3(targetFollow.position.x, transform.position.y, targetFollow.position.z);
    }
}
