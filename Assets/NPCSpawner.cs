using System;
using System.Collections;
using System.Collections.Generic;
using MidniteOilSoftware;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField]private GameObject[] NPC;

    [SerializeField] private float npcSpawnDelay = 2f;
    [SerializeField] private float _npcSpawnDelayDefault;

    private void Start()
    {
        _npcSpawnDelayDefault = npcSpawnDelay;
    }

    private void FixedUpdate()
    {
        NPCspawner();
    }

    private void NPCspawner()
    {
        if (npcSpawnDelay > 0)
        {
            npcSpawnDelay -= Time.deltaTime;
            return;
        }

        var ran = RandomSystem(0, 1);
        var ranDelay = RandomSystem(0, 10);
        if (FloodSystem.Instance.floodPoint >= 50)
        {
            ranDelay += 10;
        }
        ObjectPoolManager.SpawnGameObject(NPC[ran], transform.position, Quaternion.LookRotation(-Vector3.forward));
        
        if (LevelScaling.Instance.levelScale <= 1)
        {
            npcSpawnDelay = _npcSpawnDelayDefault+ranDelay;
        }
        if (LevelScaling.Instance.levelScale == 2)
        {
            npcSpawnDelay = (_npcSpawnDelayDefault+ranDelay)*0.5f;
        }
        if (LevelScaling.Instance.levelScale == 3)
        {
            npcSpawnDelay = (_npcSpawnDelayDefault+ranDelay)*0.75f;
        }
        if (LevelScaling.Instance.levelScale >= 4)
        {
            npcSpawnDelay = _npcSpawnDelayDefault+ranDelay;
        }
    }
    
    private static int RandomSystem(int range1, int range2)
    {
        var ranNum = Random.Range(range1, range2+1);
        return ranNum;
    }
}
