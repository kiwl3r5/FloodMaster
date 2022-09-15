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

    private void Start()
    {
        //gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        GameManager.Instance.collectible++;
        GameManager.Instance.total++;
        GameManager.Instance.collectedBarUI.fillAmount = GameManager.Instance.sumCollected / GameManager.Instance.total;
        _defaultDuration = maxDuration;
    }

    private void Update()
    {
        isInDeSpawnRange = Physics.CheckSphere(transform.position, deSpawnRange, objP.whatIsManhole);
        if (objP.numFound>0)
        {
            objP.manholeManager.CheckFull();
            isManholeFull = objP.manholeManager.isFull;
        }
        if (isInDeSpawnRange && !isManholeFull)
        {
            objP.manholeManager.currentCapacity++;
            FloodSystem.Instance.floodMulti += FloodSystem.Instance.cloggedFloodRate;
            objP.manholeManager.CloggedTrashSize();
            DespawnObj();
        }
    }

    private void FixedUpdate()
    {
        maxDuration -= Time.deltaTime;
        if (maxDuration <= 0)
        {
            ObjectPoolManager.DespawnGameObject(gameObject);
            maxDuration = _defaultDuration;
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
            GameManager.Instance.sumCollected++;
            GameManager.Instance.collectible--;
            GameManager.Instance.collectedBarUI.fillAmount = GameManager.Instance.sumCollected / GameManager.Instance.total;
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

    public void DespawnObj()
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
