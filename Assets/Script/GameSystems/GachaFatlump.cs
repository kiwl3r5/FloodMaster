using System;
using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GachaFatlump : MonoBehaviour
{
   [SerializeField] private GameObject gachaUi;
   
   [SerializeField] private Text slot1HeaderText;
   [SerializeField] private Text slot1DescripText;
   [SerializeField] private Button slot1Button;
   [FormerlySerializedAs("boatSlot1")] [SerializeField] private GameObject[] bgSlot1;
   
   [SerializeField] private Text slot2HeaderText;
   [SerializeField] private Text slot2DescripText;
   [SerializeField] private Button slot2Button;
   [SerializeField] private GameObject[] bgSlot2;
   public bool isGachaUiOn;

   private SkillSystem _skillSystem;
   
   private static GachaFatlump _instance;
   public static GachaFatlump Instance { get { return _instance; } }

   private void Awake()
   {
      _instance = this;
      _skillSystem = GetComponent<SkillSystem>();
   }

   private void Update()
   {
      if (Keyboard.current.pKey.wasPressedThisFrame)
      {
         StartCoroutine(GachaSlot());
      }
      if (gachaUi.activeInHierarchy)
      {
         isGachaUiOn = true;
      }
      if (!gachaUi.activeInHierarchy)
      {
         isGachaUiOn = false;
      }
   }

   public void StartGacha()
   {
      StartCoroutine(GachaSlot());
   }

   public IEnumerator GachaSlot()
   {
      gachaUi.SetActive(true);
      GameManager.Instance.PauseGame();
      int ranChanceSlot1 = Random.Range(1, 5);
      int ranChanceSlot2 = Random.Range(1, 5);
      //int ranChanceSlot3 = Random.Range(1, 4);
      yield return null;
      if (ranChanceSlot1 == 1)
      {
         int slot1RanBoat = Random.Range(1, 14);
         if (slot1RanBoat <= 2)
         {
            //SR_Boat
            slot1HeaderText.text = "Speed Boat";
            bgSlot1[0].SetActive(false);
            bgSlot1[1].SetActive(false);
            bgSlot1[2].SetActive(false);
            bgSlot1[3].SetActive(true);
            slot1Button.onClick.RemoveAllListeners();
            slot1Button.onClick.AddListener(BoatSkillSR);
         }
         if (slot1RanBoat is > 2 and <= 6)
         {
            //R_Boat
            slot1HeaderText.text = "Fishing Boat";
            bgSlot1[0].SetActive(false);
            bgSlot1[1].SetActive(false);
            bgSlot1[2].SetActive(true);
            bgSlot1[3].SetActive(false);
            slot1Button.onClick.RemoveAllListeners();
            slot1Button.onClick.AddListener(BoatSkillR);
         }
         if (slot1RanBoat > 6)
         {
            //N_Boat
            slot1HeaderText.text = "Wooden Boat";
            bgSlot1[0].SetActive(false);
            bgSlot1[1].SetActive(true);
            bgSlot1[2].SetActive(false);
            bgSlot1[3].SetActive(false);
            slot1Button.onClick.RemoveAllListeners();
            slot1Button.onClick.AddListener(BoatSkillN);
         }
      }
      if (ranChanceSlot1 > 1)
      {
         bgSlot1[0].SetActive(true);
         bgSlot1[1].SetActive(false);
         bgSlot1[2].SetActive(false);
         bgSlot1[3].SetActive(false);
         int slot1RanHelp = Random.Range(1, 5);
         if (slot1RanHelp == 1)
         {
            slot1HeaderText.text = "+30 Karma Points";
            slot1Button.onClick.RemoveAllListeners();
            slot1Button.onClick.AddListener(KarmaPoints);
         }
         if (slot1RanHelp == 2)
         {
            slot1HeaderText.text = "Reduce FloodLevel -20%";
            slot1Button.onClick.RemoveAllListeners();
            slot1Button.onClick.AddListener(ReduceFloodLevel);
         }
         if (slot1RanHelp == 3)
         {
            slot1HeaderText.text = "Reduce FloodRate -5%";
            slot1Button.onClick.RemoveAllListeners();
            slot1Button.onClick.AddListener(ReduceFloodRate);
         }
         if (slot1RanHelp == 4)
         {
            slot1HeaderText.text = "Free Flood Boots Skill";
            slot1Button.onClick.RemoveAllListeners();
            slot1Button.onClick.AddListener(FreeFloodBootsSkill);
         }
         if (slot1RanHelp == 5)
         {
            slot1HeaderText.text = "Free Scare Enemy Skill";
            slot1Button.onClick.RemoveAllListeners();
            slot1Button.onClick.AddListener(FreeScareEnemySkill);
         }
      }
      yield return null;
      //
      // Slot2
      //
      if (ranChanceSlot2 == 1)
      {
         int slot2RanBoat = Random.Range(1, 14);
         if (slot2RanBoat <= 2)
         {
            //SR_Boat
            slot2HeaderText.text = "Speed Boat";
            bgSlot2[0].SetActive(false);
            bgSlot2[1].SetActive(false);
            bgSlot2[2].SetActive(false);
            bgSlot2[3].SetActive(true);
            slot2Button.onClick.RemoveAllListeners();
            slot2Button.onClick.AddListener(BoatSkillSR);
         }
         if (slot2RanBoat is > 2 and <= 6)
         {
            //R_Boat
            slot2HeaderText.text = "Fishing Boat";
            bgSlot2[0].SetActive(false);
            bgSlot2[1].SetActive(false);
            bgSlot2[2].SetActive(true);
            bgSlot2[3].SetActive(false);
            slot2Button.onClick.RemoveAllListeners();
            slot2Button.onClick.AddListener(BoatSkillR);
         }
         if (slot2RanBoat > 6)
         {
            //N_Boat
            slot2HeaderText.text = "Wooden Boat";
            bgSlot2[0].SetActive(false);
            bgSlot2[1].SetActive(true);
            bgSlot2[2].SetActive(false);
            bgSlot2[3].SetActive(false);
            slot2Button.onClick.RemoveAllListeners();
            slot2Button.onClick.AddListener(BoatSkillN);
         }
      }
      if (ranChanceSlot2 > 1)
      {
         bgSlot2[0].SetActive(true);
         bgSlot2[1].SetActive(false);
         bgSlot2[2].SetActive(false);
         bgSlot2[3].SetActive(false);
         int slot2RanHelp = Random.Range(1, 5);
         if (slot2RanHelp == 1)
         {
            slot2HeaderText.text = "+30 Karma Points";
            slot2Button.onClick.RemoveAllListeners();
            slot2Button.onClick.AddListener(KarmaPoints);
         }
         if (slot2RanHelp == 2)
         {
            slot2HeaderText.text = "Reduce FloodLevel -20%";
            slot2Button.onClick.RemoveAllListeners();
            slot2Button.onClick.AddListener(ReduceFloodLevel);
         }
         if (slot2RanHelp == 3)
         {
            slot2HeaderText.text = "Reduce FloodRate -5%";
            slot2Button.onClick.RemoveAllListeners();
            slot2Button.onClick.AddListener(ReduceFloodRate);
         }
         if (slot2RanHelp == 4)
         {
            slot2HeaderText.text = "Free Flood Boots Skill";
            slot2Button.onClick.RemoveAllListeners();
            slot2Button.onClick.AddListener(FreeFloodBootsSkill);
         }
         if (slot2RanHelp == 5)
         {
            slot2HeaderText.text = "Free Scare Enemy Skill";
            slot2Button.onClick.RemoveAllListeners();
            slot2Button.onClick.AddListener(FreeScareEnemySkill);
         }
      }
   }

   private void KarmaPoints()
   {
      if (GameManager.Instance.sumKarmaPoints > 70)
      {
         GameManager.Instance.sumKarmaPoints = 100;
         GameManager.Instance.collectedBarUI.fillAmount = GameManager.Instance.sumKarmaPoints / GameManager.Instance.maxKarma;
         gachaUi.SetActive(false);
         GameManager.Instance.ResumeGame();
         return;
      }
      GameManager.Instance.sumKarmaPoints += 30;
      GameManager.Instance.collectedBarUI.fillAmount = GameManager.Instance.sumKarmaPoints / GameManager.Instance.maxKarma;
      gachaUi.SetActive(false);
      GameManager.Instance.ResumeGame();
   }
   
   private void ReduceFloodLevel()
   {
      FloodSystem.Instance.floodPoint *= 0.8f;
      gachaUi.SetActive(false);
      GameManager.Instance.ResumeGame();
   }

   private void ReduceFloodRate()
   {
      if (FloodSystem.Instance.floodMulti < FloodSystem.Instance.floodUiCalculate/100*5)
      {
         FloodSystem.Instance.floodMulti = 0;
         gachaUi.SetActive(false);
         GameManager.Instance.ResumeGame();
         return;
      }
      FloodSystem.Instance.floodMulti -= FloodSystem.Instance.floodUiCalculate/100*5;
      gachaUi.SetActive(false);
      GameManager.Instance.ResumeGame();
      StartCoroutine(FloodSystem.Instance.FloodReduce());
   }

   private void FreeFloodBootsSkill()
   {
      StartCoroutine(_skillSystem.FloodBoots());
      gachaUi.SetActive(false);
      GameManager.Instance.ResumeGame();
      StartCoroutine(FloodSystem.Instance.FloodReduce());
   }

   private void FreeScareEnemySkill()
   {
      StartCoroutine(_skillSystem.ScareEnemy());
      gachaUi.SetActive(false);
      GameManager.Instance.ResumeGame();
      StartCoroutine(FloodSystem.Instance.FloodReduce());
   }

   private void BoatSkillN()
   {
      _skillSystem.boatOwnNum = 1;
      _skillSystem.BoatSkillIconChanger(1);
      gachaUi.SetActive(false);
      GameManager.Instance.ResumeGame();
      StartCoroutine(FloodSystem.Instance.FloodReduce());
   }
   private void BoatSkillR()
   {
      _skillSystem.boatOwnNum = 2;
      _skillSystem.BoatSkillIconChanger(2);
      gachaUi.SetActive(false);
      GameManager.Instance.ResumeGame();
      StartCoroutine(FloodSystem.Instance.FloodReduce());
   }
   private void BoatSkillSR()
   {
      _skillSystem.boatOwnNum = 3;
      _skillSystem.BoatSkillIconChanger(3);
      gachaUi.SetActive(false);
      GameManager.Instance.ResumeGame();
      StartCoroutine(FloodSystem.Instance.FloodReduce());
   }
}
