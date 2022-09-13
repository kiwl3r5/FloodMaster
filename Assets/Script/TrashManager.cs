using System.Collections;
using System.Collections.Generic;
using MidniteOilSoftware;
using UnityEngine;

public class TrashManager : MonoBehaviour
{
    [SerializeField]private GameObject player;
    [SerializeField]private GameObject trash;
    [SerializeField]private Transform[] trashSpawner;
    [SerializeField]private float spawnDelay = 5;
    private float _spawnDelayDefault;
    public int ran;
    public int ranF;
    public float ranTranform = 0;
    private Vector3 _playerPosition;
    
    // Start is called before the first frame update
    private void Start()
    {
        _spawnDelayDefault = spawnDelay;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //var playerPosition = player.transform.position;
        _playerPosition = player.transform.position;
        gameObject.transform.position = new Vector3(ranTranform,0,_playerPosition.z);
        TrashSpawner();
    }

    private void TrashSpawner()
    {
        if (spawnDelay > 0)
        {
            spawnDelay -= Time.deltaTime;
            return;
        }

        ran = RandomSystem(0, 3);
        ranF = RandomSystem(4, 7);
        ranTranform = RandomSystem(-1, 1);
        ObjectPoolManager.SpawnGameObject(trash, trashSpawner[ran].position, Quaternion.identity);
        ObjectPoolManager.SpawnGameObject(trash, trashSpawner[ranF].position, Quaternion.identity);
        //Instantiate(trash, trashSpawner[ran].position,Quaternion.identity);
        
        spawnDelay = _spawnDelayDefault;

    }

    private static int RandomSystem(int range1, int range2)
    {
        var ranNum = Random.Range(range1, range2+1);
        return ranNum;
    }
}
