using System.Collections;
using Script.Manager;
using UnityEngine;
using UnityEngine.Serialization;

public class FloodSystem : MonoBehaviour
{
    public float floodPoint;
    public float floodMulti;
    public float cloggedFloodRate = 0.02f;
    public float floodUiCalculate;
    public float manholeNum;
    public GameObject water;
    public GameObject playerObj;
    private Vector3 _playerPos;
    public bool fatLumpClear;
    [FormerlySerializedAs("ReduseDuration")] [SerializeField] private float reduceDuration = 1f;
    public float reduceIntensity = 0.3f;
    [SerializeField] private float fullReduceRate = 0.8f;
    public bool isCheatOn;
    private float floodLevelThrust;


    public static FloodSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        } else {
            Instance = this;
        }
    }

    private void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        _playerPos = playerObj.transform.position;
        water = GameObject.Find("FloodWater");
    }

    private void Update()
    {
        if (fatLumpClear)
        {
            StartCoroutine(FloodReduce(reduceDuration));
            fatLumpClear = false;
        }
    }

    private void FixedUpdate()
    {
        if (isCheatOn) { return; }
        FloodCounter();
        FloodRateUi();
        _playerPos = playerObj.transform.position;
        water.transform.position= new Vector3(0,floodPoint/100*1.2f,_playerPos.z);
    }

    private void LateUpdate()
    {
        if (floodPoint >= 100)
        {
            floodPoint = 0;
            GameManager.Instance.GameOverUI(true);
        }
    }

    private void FloodCounter()
    {
        if ((floodPoint>100 && floodMulti > 0) || (floodMulti < 0 && floodPoint < -6)) { return; }
        floodPoint += Time.deltaTime * floodMulti;
        //floodBar.fillAmount = floodPoint/100;
        GameManager.Instance.floodBar.fillAmount = floodPoint/100;
    }

    private void FloodRateUi()
    {
        //floodRate.fillAmount = floodMulti / floodUiCalculate;
        if (floodMulti >= -0.2)
        {
            GameManager.Instance.floodRate.fillAmount = floodMulti / floodUiCalculate;
        }

        if (floodMulti <= 0.2)
        {
            GameManager.Instance.floodRateMin.fillAmount = floodMulti * -1 / (floodUiCalculate*fullReduceRate);
        }
    }

    private IEnumerator FloodReduce (float duration)
    {
        float elapsedTime = 0f;
        floodMulti -= cloggedFloodRate*manholeNum*fullReduceRate;
        while (elapsedTime < duration && floodPoint > 0)
        {
            elapsedTime += Time.deltaTime;
            floodPoint -= reduceIntensity+reduceIntensity*(floodMulti / floodUiCalculate);
            yield return null;
        }
    }

    public float FloodLvThrust()
    {
        if (floodPoint>20)
        {
            floodLevelThrust = 1;
        }
        if (Instance.floodPoint<0)
        {
            floodLevelThrust = 0;
        }
        if (floodPoint is <= 20 and >= 0)
        {
            floodLevelThrust = Instance.floodPoint / 20;
        }

        return floodLevelThrust;
    }
}
