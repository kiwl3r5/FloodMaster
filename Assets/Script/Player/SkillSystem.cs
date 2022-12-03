using System;
using System.Collections;
using System.Collections.Generic;
using MidniteOilSoftware;
using Script.Manager;
using Script.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SkillSystem : MonoBehaviour
{
    [Header("BootsSkill")]
    [SerializeField] private Image bootsSkillUI;
    [SerializeField] private Image bootsSkillCostUI;
    [SerializeField] private Image bootsSkillDuratUI;
    [SerializeField]private float floodBootsDuration;
    [SerializeField]private float floodBootsCosts;
    
    [Header("ScareSkill")]
    [SerializeField]private Image scareSkillUI;
    [SerializeField] private Image scareSkillCostUI;
    [SerializeField] private Image scareSkillDuratUI;
    [SerializeField] private GameObject vfxScare;
    [SerializeField]private float scareEnemyDuration;
    [SerializeField]private float scareEnemyCosts;
    
    [Header("BoatSkill")]
    [SerializeField]private Image boatSkillUI;
    [SerializeField]private Image boatSkillDuratUI;
    [SerializeField]private GameObject boatIconN;
    [SerializeField]private GameObject boatIconR;
    [SerializeField]private GameObject boatIconSr;
    [SerializeField] private Transform spawnPoint;
    public int boatOwnNum = 0;
    [SerializeField] private GameObject boatNPrefeb;
    [SerializeField] private GameObject boatRPrefeb;
    [SerializeField] private GameObject boatSrPrefeb;
    [SerializeField] private float boatSpawnDelay = 2;
    private bool _isBoatReady = true;
    public bool isSkillActive;
    
    private Coroutine _skillCoroutine;
    private GameManager _gameManager;
    
    private static SkillSystem _instance;
    public static SkillSystem Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
        _gameManager = GameManager.Instance;
        vfxScare.SetActive(false);
        BoatSkillIconChanger(boatOwnNum);
    }

    private void Update()
    {
        if (PlayerLocomotion.Instance.isDriving) { return; }
        SkillUICol(bootsSkillUI,PlayerManager.Instance.isBoots,_gameManager.sumKarmaPoints>=floodBootsCosts);
        SkillUICol(scareSkillUI,PlayerManager.Instance.isScary,_gameManager.sumKarmaPoints>=scareEnemyCosts);
        SkillUICheck();
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            if (PlayerManager.Instance.isBoots || _gameManager.sumKarmaPoints < floodBootsCosts){ return; }
            if (_skillCoroutine != null) StopCoroutine(_skillCoroutine);
            _gameManager.sumKarmaPoints -= floodBootsCosts;
            _gameManager.ReloadKarmaUI();
            _skillCoroutine = StartCoroutine(FloodBoots());
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            if (PlayerManager.Instance.isScary || _gameManager.sumKarmaPoints < scareEnemyCosts) { return; }
            if (_skillCoroutine != null) StopCoroutine(_skillCoroutine);
            _gameManager.sumKarmaPoints -= scareEnemyCosts;
            _gameManager.ReloadKarmaUI();
            _skillCoroutine = StartCoroutine(ScareEnemy());
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            if (_isBoatReady && boatOwnNum > 0)
            {
                StartCoroutine(SpawnBoat());
            }
        }
    }

    private void SkillUICheck()
    {
        bootsSkillCostUI.fillAmount = _gameManager.sumKarmaPoints / floodBootsCosts;
        scareSkillCostUI.fillAmount = _gameManager.sumKarmaPoints / scareEnemyCosts;
    }

    private static void SkillUICol(Graphic image,bool inUse,bool isUsable)
    {
        if (!inUse && isUsable)
        {
            //image.transform.LeanMoveLocalX(80, 0.4f).setEaseOutExpo();
            image.color = Color.white;
        }
        if (!inUse && !isUsable)
        {
            //image.transform.LeanMoveLocalX(115,0.4f).setEaseOutExpo();
            image.color = Color.HSVToRGB(0, 0, 0.25f);
        }
        /*bootsSkillUI.color = numColor switch
        {
            0 => Color.HSVToRGB(0, 0, 0.25f),
            1 => Color.white,
            2 => Color.yellow,
            _ => Color.white
        };*/
    }

    public IEnumerator FloodBoots()
    {
        float elapsedTime = 0f;
        PlayerManager.Instance.isScary = false;
        bootsSkillUI.transform.LeanMoveLocalX(0, 0.5f).setEaseOutExpo();
        scareSkillUI.transform.LeanMoveLocalX(80, 0.5f).setEaseOutExpo();
        while (elapsedTime < floodBootsDuration)
        {
            elapsedTime += Time.deltaTime;
            PlayerManager.Instance.isBoots = true;
            vfxScare.SetActive(false);
            bootsSkillDuratUI.fillAmount = (elapsedTime-floodBootsDuration)/-floodBootsDuration;
            Debug.Log("Start 1 Skill = "+ elapsedTime);
            yield return null;
        }
        bootsSkillUI.transform.LeanMoveLocalX(80, 0.5f).setEaseOutExpo();
        PlayerManager.Instance.isBoots = false;
        Debug.Log("1 Skill Stopped");
    }

    public IEnumerator ScareEnemy()
    {
        float elapsedTime = 0f;
        PlayerManager.Instance.isBoots = false;
        scareSkillUI.transform.LeanMoveLocalX(0, 0.5f).setEaseOutExpo();
        bootsSkillUI.transform.LeanMoveLocalX(80, 0.5f).setEaseOutExpo();
        while (elapsedTime < scareEnemyDuration)
        {
            elapsedTime += Time.deltaTime;
            PlayerManager.Instance.isScary = true;
            vfxScare.SetActive(true);
            scareSkillDuratUI.fillAmount = (elapsedTime-scareEnemyDuration)/-scareEnemyDuration;
            Debug.Log("Start 2 Skill = " + elapsedTime);
            yield return null;
        }
        scareSkillUI.transform.LeanMoveLocalX(80, 0.5f).setEaseOutExpo();
        vfxScare.SetActive(false);
        PlayerManager.Instance.isScary = false;
        Debug.Log("2 Skill Stopped");
    }

    private IEnumerator SpawnBoat()
    {
        float elapsedTime = 0f;
        if (boatOwnNum == 1)
        {
            ObjectPoolManager.SpawnGameObject(boatNPrefeb, spawnPoint.position, Quaternion.identity);
        }
        if (boatOwnNum == 2)
        {
            ObjectPoolManager.SpawnGameObject(boatRPrefeb, spawnPoint.position, Quaternion.identity);
        }
        if (boatOwnNum == 3)
        {
            ObjectPoolManager.SpawnGameObject(boatSrPrefeb, spawnPoint.position, Quaternion.identity);
        }
        while (elapsedTime < boatSpawnDelay)
        {
            elapsedTime += Time.deltaTime;
            _isBoatReady = false;
            yield return null;
        }
        _isBoatReady = true;
    }

    public void BoatSkillIconChanger(int RareNumber)
    {
        switch (RareNumber)
        {
            case 0:
                boatIconN.SetActive(false);
                boatIconR.SetActive(false);
                boatIconSr.SetActive(false);
                break;
            case 1:
                boatIconN.SetActive(true);
                boatIconR.SetActive(false);
                boatIconSr.SetActive(false);
                break;
            case 2:
                boatIconN.SetActive(false);
                boatIconR.SetActive(true);
                boatIconSr.SetActive(false);
                break;
            case 3:
                boatIconN.SetActive(false);
                boatIconR.SetActive(false);
                boatIconSr.SetActive(true);
                break;
        }
    }

    /*private static void ResetSkill()
    {
        PlayerManager.Instance.isScary = false;
        PlayerManager.Instance.isBoots = false;
    }*/
}
