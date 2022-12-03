using MidniteOilSoftware;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]private GameObject player;
    [SerializeField]private GameObject trashSmall;
    [SerializeField]private GameObject trashMedium;
    [SerializeField]private GameObject trashLarge;
    [SerializeField]private GameObject helper;
    [FormerlySerializedAs("motocycle")] [SerializeField]private GameObject motorcycle;
    [SerializeField] private GameObject boat;
    [FormerlySerializedAs("trashSpawner")] [SerializeField]private Transform[] spawner;
    [FormerlySerializedAs("spawnDelay")] [SerializeField]private float trashSpawnDelay = 1.75f;
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
        var chance = Random.value;
        ranTransform = RandomSystem(-1, 1);
        //Instantiate(trash, trashSpawner[ran].position,Quaternion.identity);
        
        if (LevelScaling.Instance.levelScale <= 1)
        {
            ObjectPoolManager.SpawnGameObject(trashSmall, spawner[ran].position, Quaternion.identity);
            ObjectPoolManager.SpawnGameObject(trashSmall, spawner[ranF].position, Quaternion.identity);
            trashSpawnDelay = _trashDelayDefault;
        }
        if (LevelScaling.Instance.levelScale == 2)
        {
            switch (chance)
            {
                case <= 0.8f:
                    ObjectPoolManager.SpawnGameObject(trashSmall, spawner[ran].position, Quaternion.identity);
                    ObjectPoolManager.SpawnGameObject(trashSmall, spawner[ranF].position, Quaternion.identity);
                    break;
                case > 0.8f:
                    ObjectPoolManager.SpawnGameObject(trashMedium, spawner[ran].position, Quaternion.identity);
                    ObjectPoolManager.SpawnGameObject(trashMedium, spawner[ranF].position, Quaternion.identity);
                    break;
            }

            trashSpawnDelay = _trashDelayDefault*0.8f;
        }
        if (LevelScaling.Instance.levelScale == 3)
        {
            switch (chance)
            {
                case <= 0.5f:
                    ObjectPoolManager.SpawnGameObject(trashSmall, spawner[ran].position, Quaternion.identity);
                    ObjectPoolManager.SpawnGameObject(trashSmall, spawner[ranF].position, Quaternion.identity);
                    break;
                case > 0.5f:
                    ObjectPoolManager.SpawnGameObject(trashMedium, spawner[ran].position, Quaternion.identity);
                    ObjectPoolManager.SpawnGameObject(trashMedium, spawner[ranF].position, Quaternion.identity);
                    break;
            }
            trashSpawnDelay = _trashDelayDefault*0.6f;
        }
        if (LevelScaling.Instance.levelScale >= 4)
        {
            switch (chance)
            {
                case <= 0.2f:
                    ObjectPoolManager.SpawnGameObject(trashSmall, spawner[ran].position, Quaternion.identity);
                    ObjectPoolManager.SpawnGameObject(trashSmall, spawner[ranF].position, Quaternion.identity);
                    break;
                case > 0.2f and <= 0.5f:
                    ObjectPoolManager.SpawnGameObject(trashMedium, spawner[ran].position, Quaternion.identity);
                    ObjectPoolManager.SpawnGameObject(trashMedium, spawner[ranF].position, Quaternion.identity);
                    break;
                case > 0.5f:
                    ObjectPoolManager.SpawnGameObject(trashLarge, spawner[ran].position, Quaternion.identity);
                    ObjectPoolManager.SpawnGameObject(trashLarge, spawner[ranF].position, Quaternion.identity);
                    break;
            }
            trashSpawnDelay = _trashDelayDefault*0.4f;
        }
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
        if (FloodSystem.Instance.floodPoint<40)
        {
            ObjectPoolManager.SpawnGameObject(motorcycle, spawner[ran].position, Quaternion.identity);
        }
        else
        {
            ObjectPoolManager.SpawnGameObject(boat, spawner[ran].position, Quaternion.identity);
        }
        obstacleSpawnDelay = _obstacleDelayDefault+ranDelay;

    }

    private static int RandomSystem(int range1, int range2)
    {
        var ranNum = Random.Range(range1, range2+1);
        return ranNum;
    }
}
