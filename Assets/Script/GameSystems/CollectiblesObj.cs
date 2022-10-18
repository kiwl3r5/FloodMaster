using System;
using System.Collections;
using System.Collections.Generic;
using MidniteOilSoftware;
using Script.Manager;
using UnityEngine;
using UnityEngine.Serialization;

public class CollectiblesObj : MonoBehaviour
{
    //public GameManager gm;
    public GameObject pickupWifi;
    public float maxDuration = 300;
    private float _defaultDuration;
    //public float cloggedFloodRate = 0.02f;
    [FormerlySerializedAs("despawnRange")] public float deSpawnRange;
    [FormerlySerializedAs("isInDespawnRange")] public bool isInDeSpawnRange;
    public bool isManholeFull = false;
    public ObjectPuller objP;
    private StormDrainManager _stormDrainManager;

    private void Start()
    {
        //gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        //GameManager.Instance.collectible++;
        //GameManager.Instance.total++;
        GameManager.Instance.collectedBarUI.fillAmount = GameManager.Instance.sumKarmaPoints / GameManager.Instance.maxKarma;
        _defaultDuration = maxDuration;
        _stormDrainManager = objP.stormDrainManager;
    }

    private void FixedUpdate()
    {
        _stormDrainManager = objP.stormDrainManager;
        isInDeSpawnRange = Physics.CheckSphere(transform.position, deSpawnRange, objP.whatIsManhole);
        if (objP.numFound>0)
        {
            _stormDrainManager.CheckFull();
            isManholeFull = _stormDrainManager.isFull;
        }
        if (isInDeSpawnRange && !isManholeFull)
        {
            if (_stormDrainManager.isClearBonus)
            { return; }
            _stormDrainManager.currentCapacity++;
            FloodSystem.Instance.floodMulti += FloodSystem.Instance.cloggedFloodRate;
            _stormDrainManager.CloggedTrashSize();
            DeSpawnObj();
        }
        maxDuration -= Time.deltaTime;
        if (maxDuration <= 0)
        {
            ObjectPoolManager.DespawnGameObject(gameObject);
            maxDuration = _defaultDuration;
        }
        if (GameManager.Instance.isEmptyPool)
        {
            DeSpawnObj();
            ObjectPoolManager.EmptyPool();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(pickupWifi, transform.position, transform.rotation);
            AudioManager.Instance.Play("Wifi");
            ObjectPoolManager.DespawnGameObject(gameObject);
            //Destroy(gameObject);
            GameManager.Instance.sumKarmaPoints++;
            //GameManager.Instance.collectible--;
            GameManager.Instance.ReloadKarmaUI();
            maxDuration = _defaultDuration;
            //isManholeFull = true;
        }

        if (other.CompareTag("DesTrash"))
        {
            ObjectPoolManager.DespawnGameObject(gameObject);
            maxDuration = _defaultDuration;
            //isManholeFull = true;
        }
        
        /*if (other.CompareTag("manholeRad"))
        {
            if (!isManholeFull)
            {
                Invoke(nameof(DespawnObj),0.5f);
            }
        }*/
    }

    private void DeSpawnObj()
    {
        ObjectPoolManager.DespawnGameObject(gameObject);
        maxDuration = _defaultDuration;
    }

    private void OnDrawGizmosSelected()
    {
        var position = transform.position;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(position, deSpawnRange);
    }
}
