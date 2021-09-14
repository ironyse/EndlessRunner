using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTemplateController : MonoBehaviour
{
    private const float DEBUG_LINE_HEIGHT = 10f;
    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position + Vector3.up * DEBUG_LINE_HEIGHT / 2, transform.position + Vector3.down * DEBUG_LINE_HEIGHT / 2, Color.green);
    }
}
