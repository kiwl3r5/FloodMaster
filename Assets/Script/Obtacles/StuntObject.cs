using System;
using System.Collections;
using System.Collections.Generic;
using Script.Player;
using UnityEngine;

public class StuntObject : MonoBehaviour
{
    private GameObject water;
    [SerializeField] private float stuntObjOffset;

    private void Awake()
    {
        water = GameObject.Find("FloodWater");
    }

    private void Update()
    {
        transform.position =
            new Vector3(transform.position.x, water.transform.position.y+stuntObjOffset, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !PlayerLocomotion.Instance.isDriving)
        {
            PlayerLocomotion.Instance.isStunt = true;
        }
    }
}
