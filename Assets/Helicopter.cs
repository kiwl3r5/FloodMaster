using System;
using System.Collections;
using System.Collections.Generic;
using MidniteOilSoftware;
using Script.Manager;
using Script.Player;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Helicopter : MonoBehaviour
{
    [SerializeField] private GameObject helicopter;
    public GameObject[] waypoints;
    [SerializeField] private GameObject supplybox;
    //private int currentLocation = 0;
    [SerializeField] private float speed;
    [SerializeField] private float WPradius = 1;
    [SerializeField] private int supplyProgress;
    private float startDelay = 3f;

    private void Update()
    {
        if (startDelay > 0)
        {
            startDelay -= Time.deltaTime;
            return;
        }
        if (LevelScaling.Instance.mapPercentage is >= 25 and <50 && supplyProgress == 0)
        {
            supplyProgress++;
            StartCoroutine(StageAndSpUI.Instance.SupplyAlert());
            StartCoroutine(DropSupply());
        }
        if (LevelScaling.Instance.mapPercentage is >= 50 and <75 && supplyProgress == 1)
        {
            supplyProgress++;
            StartCoroutine(StageAndSpUI.Instance.SupplyAlert());
            StartCoroutine(DropSupply());
        }
        if (LevelScaling.Instance.mapPercentage >= 75 && supplyProgress == 2)
        {
            supplyProgress++;
            StartCoroutine(StageAndSpUI.Instance.SupplyAlert());
            StartCoroutine(DropSupply());
        }
        if (Keyboard.current.oKey.wasPressedThisFrame && GameManager.Instance.cheatUI.activeInHierarchy)
        {
            StartCoroutine(StageAndSpUI.Instance.SupplyAlert());
            StartCoroutine(DropSupply());
        }
    }

    IEnumerator DropSupply()
    {
        helicopter.gameObject.SetActive(true);
        transform.rotation = Quaternion.LookRotation(Vector3.forward);
        yield return null;
        while (Vector3.Distance(waypoints[1].transform.position,transform.position) > WPradius)
        {
            transform.parent.position =
                new Vector3(0, transform.parent.position.y, PlayerLocomotion.Instance.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, waypoints[1].transform.position,
                Time.deltaTime * speed);
            yield return null;
        }
        yield return new WaitForSeconds(1);
        ObjectPoolManager.SpawnGameObject(supplybox,transform.position,Quaternion.identity);
        yield return new WaitForSeconds(1);
        transform.rotation = Quaternion.LookRotation(-Vector3.forward);
        while (Vector3.Distance(waypoints[0].transform.position,transform.position) > WPradius)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[0].transform.position,
                Time.deltaTime * speed);
            yield return null;
        }
        helicopter.gameObject.SetActive(false);
    }
}
