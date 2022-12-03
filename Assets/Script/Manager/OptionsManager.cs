using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Script.Manager
{
    public class OptionsManager : MonoBehaviour
    {
        public AudioMixer audioMixer;
        [SerializeField] private GameObject optionsUI;
        //public CinemachineFreeLook cinemachineFreeLook;
        //private GameObject _thirdPCam;
        [SerializeField]private float _savedSensitivity;
        public Slider audiosLevel;
        public Slider musicLevel;
        public Slider sfxLevel;
        public Slider mouseLevel;

        private void Start()
        {
            if (GameManager.Instance.sceneNum > 0)
            {
                CameraManager.Instance.sensitivity = _savedSensitivity;
            }
        }

        private void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame && optionsUI.activeInHierarchy)
            {
                CloseOptions();
            }
        }


        public void SetVolume(float volume)
        {
            audioMixer.SetFloat("Volume",volume);
        }
        
        public void SetSFX(float volume)
        {
            audioMixer.SetFloat("SFX",volume);
        }
        
        public void SetMusic(float volume)
        {
            audioMixer.SetFloat("Music",volume);
        }
        
        public void SetMouse(float sensitivity)
        {
            _savedSensitivity = sensitivity;
            if (GameManager.Instance.sceneNum>0)
            {
                CameraManager.Instance.sensitivity = sensitivity;
            }
        }
        
        public IEnumerator CinemachineSetup()
        {
            CameraManager.Instance.sensitivity = _savedSensitivity;
            yield return null;
        }

        public void CloseOptions()
        {
            optionsUI.SetActive(false);
        }

        public void ResetDefault()
        {
            audiosLevel.value = 0;
            musicLevel.value = 0;
            sfxLevel.value = 0;
            mouseLevel.value = 4;
        }
    }
}