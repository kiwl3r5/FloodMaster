using System;
using System.Collections;
using System.Collections.Generic;
using MidniteOilSoftware;
using Script.Manager;
using Script.Player;
using UnityEngine;

public class FloodFloatObjManager : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    private float _lifeTimeDefault;

    private void Awake()
    {
        _lifeTimeDefault = lifeTime;
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.isEmptyPool)
        {
            ObjectPoolManager.DespawnGameObject(gameObject);
            ObjectPoolManager.EmptyPool();
            return;
        }
        
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            ObjectPoolManager.DespawnGameObject(gameObject);
            lifeTime = _lifeTimeDefault;
        }
    }
}
