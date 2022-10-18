using System;
using System.Collections;
using Script.Manager;
using Script.Player;
using UnityEngine;
using Cinemachine;

namespace Script
{
    public class SewerScript : MonoBehaviour,IInteractable
    {
        public Vector3 sewerEntryPoint;
        //public Quaternion sewerEntryPointRotete;
        [SerializeField] private FatLumpScript _fatLumpScript;
        private CinemachineFreeLook _cinemachineFreeLook;
        private GameObject _thirdPersonCam;

        private void Awake()
        {
            _thirdPersonCam = GameObject.Find("ThirdPersonCam");
        }

        private void Start()
        {
            _fatLumpScript = gameObject.GetComponentInChildren<FatLumpScript>();
            _cinemachineFreeLook = _thirdPersonCam.GetComponent<CinemachineFreeLook>();
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
            return false;
        }

        public bool Interact1(Interactor interactor)
        {
            GameManager.Instance.MiniMapUI(true);
            PlayerLocomotion.Instance.transform.position = sewerEntryPoint;
            PlayerLocomotion.Instance.transform.rotation = Quaternion.Euler(Vector3.forward);
            StartCoroutine(ResetCam());
            return false;
        }
    }
}