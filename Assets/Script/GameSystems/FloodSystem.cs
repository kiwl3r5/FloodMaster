using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloodSystem : MonoBehaviour
{
    public float floodPoint = 0;
    public float floodMulti = 0;
    public float cloggedFloodRate = 0.02f;
    public Image floodBar;
    public Image floodRate;
    public GameObject water;
    public GameObject playerObj;
    private Vector3 _playerPos;
    
    
    
    private static FloodSystem _instance;
    public static FloodSystem Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        playerObj = GameObject.FindGameObjectWithTag("Player");
        _playerPos = playerObj.transform.position;
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        FloodCounter();
        FloodRateUi();
        _playerPos = playerObj.transform.position;
        water.transform.position= new Vector3(0,floodPoint/100*1.2f,_playerPos.z);
    }

    private void FloodCounter()
    {
        if (floodPoint>100) { return; }
        floodPoint += Time.deltaTime * floodMulti;
        floodBar.fillAmount = floodPoint/100;
    }

    private void FloodRateUi()
    {
        floodRate.fillAmount = floodMulti / 2.1f;
    }
}
