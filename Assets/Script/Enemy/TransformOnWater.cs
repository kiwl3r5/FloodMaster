using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformOnWater : MonoBehaviour
{

    [SerializeField] private GameObject water;

    private void Start()
    {
        water = GameObject.Find("FloodWater");
    }

    private void FixedUpdate()
    {
        if (FloodSystem.Instance.floodPoint>20)
        {
            transform.position = new Vector3(transform.parent.position.x, water.transform.position.y-0.05f,transform.parent.position.z);   
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
    }
}
