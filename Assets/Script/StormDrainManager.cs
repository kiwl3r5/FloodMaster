using System;
using System.Collections;
using Cinemachine;
using Script;
using Script.Manager;
using Script.Player;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class StormDrainManager : MonoBehaviour, IInteractable
{
    public float maxCapacity = 10;
    public float currentCapacity;
    public float capacity;

    public bool isFull;
    public bool isFatLumpClear;
    public bool isClearBonus;
    public bool outOfSpawn = true;
    [SerializeField] private GameObject cloggedTrash;
    [SerializeField] private GameObject vortex;
    [SerializeField] private GameObject vortexStop;
    [SerializeField] private GameObject sewer;
    [SerializeField] private GameObject sewerEnterPoint;
    [SerializeField] private SewerScript sewerScript;
    [SerializeField] private FatLumpScript fatLumpScript;
    [SerializeField] private Vector3 stormDrainPosition;
    [SerializeField] private float vortexPosOffsetY = 0.1f;
    [SerializeField] private float outOfBoundTrashDelay = 8f;
    [SerializeField] private float clearBonusDuration = 180f;
    [SerializeField] private GameObject clearBonusUi;
    [SerializeField] private GameObject SewerCrearUi;
    [SerializeField] private Image clearBonusUiDura;
    public bool clearBonusTrigger;
    private float _defaultOutOfBoundTrashDelay;
    private CinemachineFreeLook _cinemachineFreeLook;
    private GameObject _thirdPersonCam;
    
    private void Awake()
    {
        _thirdPersonCam = GameObject.Find("ThirdPersonCam");
    }

    private void Start()
    {
        FloodSystem.Instance.manholeNum++;
        FloodSystem.Instance.floodUiCalculate = FloodSystem.Instance.cloggedFloodRate * maxCapacity * FloodSystem.Instance.manholeNum;
        clearBonusUi.SetActive(false);
        SewerCrearUi.SetActive(false);
        _cinemachineFreeLook = _thirdPersonCam.GetComponent<CinemachineFreeLook>();
        capacity = maxCapacity;
        cloggedTrash.transform.localScale = new Vector3(currentCapacity/10, currentCapacity/10, currentCapacity/10);
        sewer = GameObject.Find("Sewer");
        sewerEnterPoint = GameObject.Find("EnterPoint");
        sewerScript = sewer.GetComponentInChildren<SewerScript>();
        fatLumpScript = sewer.GetComponentInChildren<FatLumpScript>();
        _defaultOutOfBoundTrashDelay = outOfBoundTrashDelay;
        stormDrainPosition = gameObject.transform.position;
        outOfBoundTrashDelay = _defaultOutOfBoundTrashDelay+Random.Range(0,20);
    }

    private void Update()
    {
        if (clearBonusTrigger)
        {
            StartCoroutine(ClearBonus());
            SewerCrearUi.SetActive(true);
            clearBonusTrigger = false;
        }
        if (currentCapacity >= capacity || isClearBonus)
        {
            vortex.SetActive(false);
            vortexStop.SetActive(true);
            if (isClearBonus)
            {
                eKeyEnable = !(currentCapacity <= 0);
            }
        }
        else
        {
            vortex.SetActive(true);
            vortexStop.SetActive(false);
            switch (currentCapacity)
            {
                case 0:
                    eKeyEnable = false;
                    //fKeyEnable = true;
                    break;
                case > 0:
                    eKeyEnable = true;
                    //fKeyEnable = false;
                    break;
            }
        }

        if (isClearBonus) { return; }
        if (outOfSpawn && currentCapacity < maxCapacity)
        {
            if (outOfBoundTrashDelay > 0)
            {
                outOfBoundTrashDelay -= Time.deltaTime;
                return;
            }
            currentCapacity++;
            FloodSystem.Instance.floodMulti += FloodSystem.Instance.cloggedFloodRate;
            CloggedTrashSize();
            outOfBoundTrashDelay = _defaultOutOfBoundTrashDelay+Random.Range(0,20);
        }
    }
    
    private void FixedUpdate()
    {
        var vortexTransform = vortex.transform.position;
        var vortexNewPosition = new Vector3(vortexTransform.x, FloodSystem.Instance.water.transform.position.y+vortexPosOffsetY, vortexTransform.z);
        vortex.transform.position = vortexNewPosition;
        vortexStop.transform.position = vortexNewPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HelperObj"))
        {
            if (currentCapacity>0)
            {
                GameManager.Instance.sumKarmaPoints += currentCapacity;
                GameManager.Instance.collectedBarUI.fillAmount = GameManager.Instance.sumKarmaPoints / GameManager.Instance.maxKarma;
                FloodSystem.Instance.floodMulti -= FloodSystem.Instance.cloggedFloodRate*currentCapacity;
                currentCapacity = 0;
                CloggedTrashSize();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("NoSewerSpawn"))
        {
            outOfSpawn = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NoSewerSpawn"))
        {
            outOfSpawn = true;
        }
    }
    
    public IEnumerator ClearBonus()
    {
        float elapsedTime = 0f;
        clearBonusUi.SetActive(true);
        while (elapsedTime < clearBonusDuration)
        {
            elapsedTime += Time.deltaTime;
            clearBonusUiDura.fillAmount = (elapsedTime - clearBonusDuration) / -clearBonusDuration;
            isClearBonus = true;
            yield return null;
        }
        isClearBonus = false;
        clearBonusUi.SetActive(false);
        Debug.Log("ClearBonus Stopped");
    }

    public void CloggedTrashSize()
    {
        cloggedTrash.transform.localScale = new Vector3(currentCapacity/10, currentCapacity/10, currentCapacity/10);
    }
    
    public void CheckFull()
    {
        isFull = currentCapacity >= capacity;
    }

    public void ClearCheckPrompt()
    {
        prompt1 = "Get in the Sewer (Clear)";
    }
    
    private IEnumerator ResetCam()
    {
        _cinemachineFreeLook.m_RecenterToTargetHeading.m_enabled = true;
        yield return new WaitForSeconds(0.5f);
        _cinemachineFreeLook.m_RecenterToTargetHeading.m_enabled = false;
    }

    [Header("Interact")]
    [SerializeField] private bool eKeyEnable;
    [SerializeField] private string prompt;
    [SerializeField] private bool fKeyEnable;
    [SerializeField] private string prompt1;
    public string InteractionPrompt => prompt;
    public string InteractionPrompt1 => prompt1;
    public bool IsEKeyEnable => eKeyEnable;
    public bool IsFKeyEnable => fKeyEnable;

    public bool Interact(Interactor interactor)
    {
        if (currentCapacity>0)
        {
            currentCapacity--;
            ScoreManager.Instance.rawScore += 100;
            GameManager.Instance.sumKarmaPoints++;
            GameManager.Instance.collectedBarUI.fillAmount = GameManager.Instance.sumKarmaPoints / GameManager.Instance.maxKarma;
            FloodSystem.Instance.floodMulti -= FloodSystem.Instance.cloggedFloodRate;
            CloggedTrashSize();
            PlayerLocomotion.Instance.LookAtTarget(transform.position);
            if (!PlayerManager.Instance.isInteracting)
            {
                AnimatorManager.Instance.PlayTargetAnimation("Interact",true,0.2f);
                PlayerLocomotion.Instance.playerRigidbody.velocity = Vector3.zero;
            }
            return true;
        }
        
        return false;
    }

    public bool Interact1(Interactor interactor)
    {
        if (!isFatLumpClear)
        {
            fatLumpScript.gameObject.SetActive(true);
            fatLumpScript.ResetFatLump();
        }
        fatLumpScript.isClear = isFatLumpClear;

        StartCoroutine(ResetCam());
        GameManager.Instance.MiniMapUI(false);
        PlayerLocomotion.Instance.gameObject.transform.position = sewerEnterPoint.transform.position;
        PlayerLocomotion.Instance.transform.rotation = Quaternion.Euler(Vector3.forward);
        sewerScript.sewerEntryPoint = stormDrainPosition;
        return false;
    }
}
