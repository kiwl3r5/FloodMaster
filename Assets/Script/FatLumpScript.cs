using System;
using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using Script.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script
{
    public class FatLumpScript : MonoBehaviour,IInteractable
    {
        [SerializeField] private SewerScript _sewerScript;
        public float fatLumpHP = 100f;
        public float defaultfatLumpHP;
        private Vector3 defaultPosition;
        [SerializeField]private float fatLumpDMGReceived = 5f;
        [SerializeField]private float fatLumpShakeIntensity = 0.3f;
        //[SerializeField]private float floodReduceDuration = 1f;
        private float totalDMGReceived;
        public bool isClear;

        private void Start()
        {
            _sewerScript = gameObject.GetComponentInParent<SewerScript>();
            defaultfatLumpHP = fatLumpHP;
            defaultPosition = transform.position;
        }

        private void Update()
        {
            if (fatLumpHP < 100)
            {
                transform.position = defaultPosition + Random.insideUnitSphere * (totalDMGReceived/100*fatLumpShakeIntensity);
            }
        }

        public void ResetFatLump()
        {
            fatLumpHP = defaultfatLumpHP;
            totalDMGReceived = 0f;
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
            throw new NotImplementedException();
        }

        public bool Interact1(Interactor interactor)
        {
            if (fatLumpHP > 0 && totalDMGReceived < 100)
            {
                fatLumpHP -= fatLumpDMGReceived;
                totalDMGReceived += fatLumpDMGReceived;
                PlayerLocomotion.Instance.LookAtTarget(transform.position);
                if (!PlayerManager.Instance.isInteracting)
                {
                    AnimatorManager.Instance.PlayTargetAnimation("Punch",true,0.2f);
                    PlayerLocomotion.Instance.playerRigidbody.velocity = Vector3.zero;
                }
                if (fatLumpHP<=0)
                {
                    //FloodSystem.Instance.fatLumpClear = true;
                    isClear = true;
                    PlayerManager.Instance.stormDrainManager.isFatLumpClear = true;
                    PlayerManager.Instance.stormDrainManager.clearBonusTrigger = true;
                    PlayerManager.Instance.stormDrainManager.ClearCheckPrompt();
                    GameManager.Instance.sumKarmaPoints += 10f;
                    GameManager.Instance.rawScore += 500;
                    GameManager.Instance.collectedBarUI.fillAmount = GameManager.Instance.sumKarmaPoints / GameManager.Instance.maxKarma;
                    gameObject.SetActive(false);
                    GachaFatlump.Instance.StartGacha();
                    AudioManager.Instance.Play("Eggplotion");
                }
            }
            return false;
        }
    }
}