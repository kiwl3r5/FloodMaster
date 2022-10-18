using System.Collections;
using System.Collections.Generic;
using MidniteOilSoftware;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]private GameObject player;
    [SerializeField]private GameObject trash;
    [SerializeField]private GameObject helper;
    [SerializeField]private GameObject Motocycle;
    [FormerlySerializedAs("trashSpawner")] [SerializeField]private Transform[] spawner;
    [FormerlySerializedAs("spawnDelay")] [SerializeField]private float trashSpawnDelay = 5;
    [SerializeField]private float helperSpawnDelay = 10;
    [SerializeField] private float obstacleSpawnDelay = 20;
    private float _trashDelayDefault;
    private float _helperDelayDefault;
    private float _obstacleDelayDefault;
    [FormerlySerializedAs("ranTranform")] public float ranTransform;
    private Vector3 _playerPosition;
    
    // Start is called before the first frame update
    private void Start()
    {
        _trashDelayDefault = trashSpawnDelay;
        _helperDelayDefault = helperSpawnDelay;
        _obstacleDelayDefault = obstacleSpawnDelay;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //var playerPosition = player.transform.position;
        _playerPosition = player.transform.position;
        gameObject.transform.position = new Vector3(ranTransform,0,_playerPosition.z);
        TrashSpawner();
        if (FloodSystem.Instance.floodPoint>=25)
        {
            HelperSpawner();
        }

        if (FloodSystem.Instance.floodPoint <= 75)
        {
            ObstacleSpawner();
        }
    }

    private void TrashSpawner()
    {
        if (trashSpawnDelay > 0)
        {
            trashSpawnDelay -= Time.deltaTime;
            return;
        }

        var ran = RandomSystem(0, 3);
        var ranF = RandomSystem(4, 7);
        ranTransform = RandomSystem(-1, 1);
        ObjectPoolManager.SpawnGameObject(trash, spawner[ran].position, Quaternion.identity);
        ObjectPoolManager.SpawnGameObject(trash, spawner[ranF].position, Quaternion.identity);
        //Instantiate(trash, trashSpawner[ran].position,Quaternion.identity);
        
        trashSpawnDelay = _trashDelayDefault;

    }
    
    private void HelperSpawner()
    {
        if (helperSpawnDelay > 0)
        {
            helperSpawnDelay -= Time.deltaTime;
            return;
        }

        var ran = RandomSystem(0, 3);
        var ranDelay = RandomSystem(0, 10);
        ObjectPoolManager.SpawnGameObject(helper, spawner[ran].position, Quaternion.identity);
        
        helperSpawnDelay = _helperDelayDefault+ranDelay;
        switch (FloodSystem.Instance.floodPoint)
        {
            case >= 50 and < 75:
                helperSpawnDelay -= 10;
                break;
            case >= 75:
                helperSpawnDelay -= 20;
                break;
        }
    }
    
    private void ObstacleSpawner()
    {
        if (obstacleSpawnDelay > 0)
        {
            obstacleSpawnDelay -= Time.deltaTime;
            return;
        }

        var ran = RandomSystem(5, 6);
        var ranDelay = RandomSystem(0, 15);
        ObjectPoolManager.SpawnGameObject(Motocycle, spawner[ran].position, Quaternion.identity);
        obstacleSpawnDelay = _obstacleDelayDefault+ranDelay;

    }

    private static int RandomSystem(int range1, int range2)
    {
        var ranNum = Random.Range(range1, range2+1);
        return ranNum;
    }
}
