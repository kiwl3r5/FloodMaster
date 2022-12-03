using System;
using System.Collections;
using System.Collections.Generic;
using MidniteOilSoftware;
using Script.Manager;
using Script.Player;
using UnityEngine;
using Random = UnityEngine.Random;

public class FloodObjSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] floodObj;
    [SerializeField]private float floodObjSpawnDelay = 1.75f;
    [SerializeField] private Vector3 spawnLocation;
    [SerializeField] private float playerDistance;
    private float _floodObjSpawnDelayDefault;
    public int Lenght;

    private void Awake()
    {
        _floodObjSpawnDelayDefault = floodObjSpawnDelay;
        Lenght = floodObj.Length;
        spawnLocation = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.Instance.sceneNum > 0 && !GameManager.Instance.isLoading)
        {
            playerDistance = Vector3.Distance(PlayerLocomotion.Instance.transform.position, transform.position);
            if (FloodSystem.Instance.floodPoint >= 30 && playerDistance < 40)
            {
                FloodObjSpawn(0);
            }
        }
    }

    private void FloodObjSpawn(float delay)
    {
        var ranSpawn = Random.Range(-1f, 1f);
        Vector3 spawnLocationRan = new Vector3(spawnLocation.x+ranSpawn,spawnLocation.y,spawnLocation.z);
        if (floodObjSpawnDelay > 0)
        {
            floodObjSpawnDelay -= Time.deltaTime;
            return;
        }
        var ranObj = Random.Range(0, floodObj.Length);
        if (FloodSystem.Instance.floodPoint <= 50)
        {
            delay += Random.Range(5,10);
        }
        if (FloodSystem.Instance.floodPoint > 50)
        {
            delay += Random.Range(0,2);
        }
        ObjectPoolManager.SpawnGameObject(floodObj[ranObj], spawnLocationRan, Quaternion.identity);
        floodObjSpawnDelay = _floodObjSpawnDelayDefault+delay;
    }
}
