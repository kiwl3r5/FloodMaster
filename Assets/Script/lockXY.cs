using System;
using UnityEngine;

public class lockXY : MonoBehaviour
{
    private Vector3 iniRot;
    private void Start()
    {
        iniRot = transform.eulerAngles;
    }

    private void LateUpdate()
    {
        iniRot.y = transform.eulerAngles.y;
        transform.eulerAngles = iniRot;
    }
}