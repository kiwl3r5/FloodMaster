using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.Player
{
    public class InteractionPromptUI : MonoBehaviour
    {
        [SerializeField]private GameObject _uiPanel;
        [SerializeField]private TextMeshProUGUI _promptText;

        private void Start()
        {
            _uiPanel.SetActive(false);
        }

        public bool IsDisplayed = false;

        public void Setup(string promptText)
        {
            _promptText.text = promptText;
            _uiPanel.SetActive(true);
            IsDisplayed = true;
        }

        public void Close()
        {
            _uiPanel.SetActive(false);
            IsDisplayed = false;
        }
    }
}