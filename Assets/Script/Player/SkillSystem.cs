using System;
using System.Collections;
using System.Collections.Generic;
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
    private Coroutine _skillCoroutine;
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        vfxScare.SetActive(false);
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
            _skillCoroutine = StartCoroutine(FloodBoots());
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            if (PlayerManager.Instance.isScary || _gameManager.sumKarmaPoints < scareEnemyCosts) { return; }
            if (_skillCoroutine != null) StopCoroutine(_skillCoroutine);
            _skillCoroutine = StartCoroutine(ScareEnemy());
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
            image.color = Color.white;
        }
        if (!inUse && !isUsable)
        {
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

    private IEnumerator FloodBoots()
    {
        float elapsedTime = 0f;
        PlayerManager.Instance.isScary = false;
        bootsSkillUI.transform.LeanMoveLocalX(0, 0.5f).setEaseOutExpo();
        scareSkillUI.transform.LeanMoveLocalX(80, 0.5f).setEaseOutExpo();
        _gameManager.sumKarmaPoints -= floodBootsCosts;
        _gameManager.ReloadKarmaUI();
        while (elapsedTime < floodBootsDuration)
        {
            elapsedTime += Time.deltaTime;
            PlayerManager.Instance.isBoots = true;
            bootsSkillDuratUI.fillAmount = (elapsedTime-floodBootsDuration)/-floodBootsDuration;
            Debug.Log("Start 1 Skill = "+ elapsedTime);
            yield return null;
        }
        bootsSkillUI.transform.LeanMoveLocalX(80, 0.5f).setEaseOutExpo();
        PlayerManager.Instance.isBoots = false;
        Debug.Log("1 Skill Stopped");
    }

    private IEnumerator ScareEnemy()
    {
        float elapsedTime = 0f;
        PlayerManager.Instance.isBoots = false;
        scareSkillUI.transform.LeanMoveLocalX(0, 0.5f).setEaseOutExpo();
        bootsSkillUI.transform.LeanMoveLocalX(80, 0.5f).setEaseOutExpo();
        _gameManager.sumKarmaPoints -= scareEnemyCosts;
        _gameManager.ReloadKarmaUI();
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

    /*private static void ResetSkill()
    {
        PlayerManager.Instance.isScary = false;
        PlayerManager.Instance.isBoots = false;
    }*/
}
