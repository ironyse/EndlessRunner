using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Position")]
    public Transform player;
    public float horizontalOffset;
    
    void Update(){
        Vector3 newPos = transform.position;
        newPos.x = player.position.x + horizontalOffset;
        transform.position = newPos;
    }
}
