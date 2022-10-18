using System;
using System.Collections;
using System.Collections.Generic;
using MidniteOilSoftware;
using Script.Manager;
using Script.Player;
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{

    private Rigidbody rb;
    [SerializeField] private float speed; 

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.isEmptyPool)
        {
            ObjectPoolManager.DespawnGameObject(gameObject);
            ObjectPoolManager.EmptyPool();
        }
        rb.velocity = -Vector3.forward*speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DesTrash"))
        {
            ObjectPoolManager.DespawnGameObject(gameObject);
        }
    }
}
