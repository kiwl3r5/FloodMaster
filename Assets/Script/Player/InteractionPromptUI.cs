using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.Player
{
    public class InteractionPromptUI : MonoBehaviour
    {
        [SerializeField]private GameObject _uiPanel;
        [SerializeField]private GameObject _fKeyUiPanel;
        [SerializeField]private TextMeshProUGUI _promptText;
        [SerializeField]private TextMeshProUGUI _fKeyPromptText;

        private void Start()
        {
            _uiPanel.SetActive(false);
            _fKeyUiPanel.SetActive(false);
        }

        public bool IsDisplayed = false;

        public void Setup(string promptText,string fPromptText,bool eEnable,bool fEnable)
        {
            if (eEnable)
            {
                _promptText.text = promptText;
                _uiPanel.SetActive(true);
            }
            if (fEnable)
            {
                _fKeyPromptText.text = fPromptText;
                _fKeyUiPanel.SetActive(true);
            }

            if (!eEnable && !fEnable) { return; }
            IsDisplayed = true;
        }

        public void ReCheckPrompt(string ePromptText, string fPromptText)
        {
            _promptText.text = ePromptText;
            _fKeyPromptText.text = fPromptText;
        }
        
        public void ReCheckEnable(bool enableE,bool enableF)
        {
            _uiPanel.SetActive(enableE);
            _fKeyUiPanel.SetActive(enableF);
        }

        public void Close()
        {
            _uiPanel.SetActive(false);
            _fKeyUiPanel.SetActive(false);
            IsDisplayed = false;
        }
    }
}