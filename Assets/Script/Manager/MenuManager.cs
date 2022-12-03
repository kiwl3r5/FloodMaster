using UnityEngine;
using UnityEngine.UI;

namespace Script.Manager
{
    public class MenuManager : MonoBehaviour
    {
        public Button startButton;
        public Button optionsButton;
        public Button quitButton;
        public GameObject creditsPanel;
        public Button creditsOpen;
        public Button creditsClose;
        [SerializeField]private bool isCreditsOpen;
        private void Awake()
        {
            Debug.Assert(startButton!=null,"startButton!=null");
            isCreditsOpen = false;
            quitButton.onClick.AddListener(Quit);
            startButton.onClick.AddListener(StartGame);
            optionsButton.onClick.AddListener(Options);
            creditsOpen.onClick.AddListener(OpenCloseCredits);
            creditsClose.onClick.AddListener(OpenCloseCredits);
            GameManager.Instance.FloodUI(false);
            GameManager.Instance.MiniMapUI(false);
        }

        private static void StartGame()
        {
            GameManager.Instance.LoadLevel(1);
            Cursor.lockState = CursorLockMode.Locked;
            GameManager.Instance.objectiveUI.gameObject.SetActive(true);
            GameManager.Instance.realTimeScoreText.gameObject.SetActive(true);
            GameManager.Instance.FloodUI(true);
            GameManager.Instance.MiniMapUI(true);
            GameManager.Instance.sceneNum = 1;
            AudioManager.Instance.Play("Theme1");
        }
        
        private static void Options()
        {
            GameManager.Instance.OptionsUI();
        }

        private static void Quit()
        {
            Debug.Log("Quit Game");
            Application.Quit();
        }

        private void OpenCloseCredits()
        {
            switch (isCreditsOpen)
            {
                case true:
                    creditsPanel.gameObject.SetActive(false);
                    isCreditsOpen = false;
                    break;
                case false:
                    creditsPanel.gameObject.SetActive(true);
                    isCreditsOpen = true;
                    break;
            }
        }
    }
}
