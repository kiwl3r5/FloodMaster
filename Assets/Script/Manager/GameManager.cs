using System.Collections;
using Script.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("GameStatus")]
        public bool gameIsPause;
        public bool gameIsWin;
        public bool gameIsLose;
        public bool isWinUnlock;
        public float collectible;
        public float total;
        public float sumCollected;
        public int sceneNum;
        [SerializeField] private float scoreMax = 10000;
        [SerializeField] private float curScore;
        public float totalPlaytime;
        public string roundedScore;

        [Header("UI")]
        public GameObject deadUI;
        public GameObject pauseUI;
        public GameObject cheatUI;
        public Text returnToText;
        public GameObject objectiveUI;
        public GameObject lvCompleteUI;
        public GameObject winUI;
        public GameObject speedUpUI;
        public GameObject invincUI;
        public GameObject takeDmgUI;
        public Text scoreText;
        public Text realTimeScoreText;
        public Text totalPlayTimeText;
        public Image collectedBarUI;
        public Button nextLevelButton;
        public Image loadingScreen;
        public Image loadingBarUI;
        public Button[] restartButtons;
        public Button[] mainMenuButtons;
        public Button[] quitButtons;
        public Toggle superSpeed;
        public Toggle invincible;
        public bool isInvincibleCheatOn;
        public Toggle superJump;
        public Button instantDownload;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            } else {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            Debug.Assert(restartButtons!=null,"restartButtons!=null");
            Debug.Assert(mainMenuButtons!=null,"mainMenuButtons!=");
            Debug.Assert(quitButtons!=null,"quitButtons!=");
            ButtonSetup(restartButtons);
            ButtonSetup(mainMenuButtons);
            ButtonSetup(quitButtons);
            nextLevelButton.onClick.AddListener(LoadLastLevel);
            superSpeed.onValueChanged.AddListener(delegate {SuperSpeedCheat(superSpeed);});
            invincible.onValueChanged.AddListener(delegate {InvincibleCheat(invincible);});
            superJump.onValueChanged.AddListener(delegate {SuperJumpCheat(superJump);});
            instantDownload.onClick.AddListener(InstantDownloadCheat);
            curScore = scoreMax;
            sceneNum = SceneManager.GetActiveScene().buildIndex;
        }

        private void Start()
        {
            returnToText = returnToText.GetComponent<Text>();
            scoreText = scoreText.GetComponent<Text>();
            totalPlayTimeText = totalPlayTimeText.GetComponent<Text>();
            objectiveUI.gameObject.SetActive(sceneNum != 0);
            // FindObjectOfType<AudioManager>().Play("Theme01");
        }

        private void Update()
        {
            PauseMenuInput();
            CollectibleCheck();
            if (!gameIsPause&&sceneNum!=0)
            {
                totalPlaytime += Time.deltaTime;
                curScore -= Time.deltaTime;
                roundedScore = curScore.ToString("0000");
                realTimeScoreText.text = TimeFormatter(totalPlaytime);
            }

            if (Input.GetKey(KeyCode.C)&&Input.GetKey(KeyCode.H)&&Input.GetKey(KeyCode.E)&&Input.GetKey(KeyCode.A)&&Input.GetKey(KeyCode.T))
            {
                cheatUI.gameObject.SetActive(true);
            }
        }

        private void CollectibleCheck()
        {
            if (collectible <= 0)
            {
                isWinUnlock = true;
                returnToText.text = "Collected Item";
            }
            else
            {
                //objectiveOutline.enabled = false;
                
                isWinUnlock = false;
                returnToText.text = "Collect Item";
            }
        }
        #region CheatCode <========================================================================================
        private void SuperSpeedCheat(Toggle tgValue)
        {
            switch (tgValue.isOn)
            {
                case true:
                    PlayerLocomotion.Instance.movementSpeed *= 3;
                    PlayerLocomotion.Instance.sprintingSpeed *= 3;
                    break;
                case false:
                    PlayerLocomotion.Instance.movementSpeed /= 3;
                    PlayerLocomotion.Instance.sprintingSpeed /= 3;
                    break;
            }
        }
        private void SuperJumpCheat(Toggle tgValue)
        {
            switch (tgValue.isOn)
            {
                case true:
                    PlayerLocomotion.Instance.jumpHeight *= 4;
                    break;
                case false:
                    PlayerLocomotion.Instance.jumpHeight /= 4;
                    break;
            }
        }
        private void InvincibleCheat(Toggle tgValue)
        {
            switch (tgValue.isOn)
            {
                case true:
                    PlayerManager.Instance.godmode = true;
                    isInvincibleCheatOn = true;
                    invincUI.SetActive(true);
                    break;
                case false:
                    PlayerManager.Instance.godmode = false;
                    isInvincibleCheatOn = false;
                    invincUI.SetActive(false);
                    break;
            }
        }
        private void InstantDownloadCheat()
        {
            sumCollected = total;
            collectible = 0;
            collectedBarUI.fillAmount = sumCollected / total;
        }
        #endregion //========================================================================================
        #region GameUI <========================================================================================
        private void PauseMenuInput()
        {
            if (!Input.GetKeyDown(KeyCode.Escape) || gameIsWin || gameIsLose || sceneNum == 0) return;
            Pause_UI(!gameIsPause);
        }
        private void Pause_UI (bool check)
        {
            pauseUI.SetActive(check);
            switch (check)
            {
                case true:
                    PauseGame();
                    break;
                case false:
                    ResumeGame();
                    break;
            }
        }
        public void GameOverUI(bool check)
        {
            deadUI.SetActive(check);
            gameIsLose = check;
            switch (check)
            {
                case true:
                    PauseGame();
                    break;
                case false:
                    ResumeGame();
                    break;
            }
        }
        public void LevelCompleteUI(bool check)
        {
            lvCompleteUI.SetActive(check);
            if (!check)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
                ResetPowerUp();
            }
        }
        public void GameWinUI(bool check)
        {
            winUI.SetActive(check);
            gameIsWin = check;
            switch (check)
            {
                case true:
                    PauseGame();
                    scoreText.text = $"{roundedScore} point";
                    totalPlayTimeText.text = TimeFormatter(totalPlaytime);
                    break;
                case false:
                    ResumeGame();
                    break;
            }
        }

        public void ResetPowerUp()
        {
            SpeedUpUI(false);
            InvincUI(false);
        }

        public void SpeedUpUI(bool check)
        {
            speedUpUI.SetActive(check);
        }
        
        public void InvincUI(bool check)
        {
            invincUI.SetActive(check);
        }
        
        public void TakeDmgUI(bool check)
        {
            takeDmgUI.SetActive(check);
        }
        
        #endregion //========================================================================================

        #region SceneControl <===================================================================================

        private void PauseGame()
        {
            gameIsPause = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
        private void ResumeGame()
        {
            Time.timeScale = 1f;
            gameIsPause = false;
            if (sceneNum > 0)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        public void Restart()
        {
            ResetCollectibleN();
            ResetPowerUp();
            if (gameIsWin)
            {
                sceneNum = 1;
                totalPlaytime = 0;
                LoadLevel(1);
                GameWinUI(false);
            }
            if (gameIsLose)
            {
                LoadLevel(sceneNum);
                GameOverUI(false);
            }
            else
            {
                LoadLevel(sceneNum);
                Pause_UI(false);
            }
        }
        public void QuitGame()
        {
            Debug.Log("Quit Game!");
            Application.Quit();
        }
        #endregion //=======================================================================================

        #region LevelLoader <========================================================================================
        private void ToMenu()
        {
            sceneNum = 0;
            curScore = scoreMax;
            totalPlaytime = 0;
            Pause_UI(false);
            GameWinUI(false);
            GameOverUI(false);
            TakeDmgUI(false);
            LoadLevel(0);
            Cursor.lockState = CursorLockMode.None;
            realTimeScoreText.gameObject.SetActive(false);
            objectiveUI.gameObject.SetActive(false);
            ResetPowerUp();
            
            AudioManager.Instance.Stop("Theme1");
            //Invoke(nameof(DestroyGameObj),0.1f);
        }
        private void LoadLastLevel()
        {
            LoadLevel(2);
            sceneNum = 2;
            LevelCompleteUI(false);
        }
        
        public void LoadLevel(int sceneIndex)
        {
            StartCoroutine(LoadAsynchronously(sceneIndex));
            ResetCollectibleN();
        }

        private IEnumerator LoadAsynchronously(int sceneIndex)
        {
            var operation = SceneManager.LoadSceneAsync(sceneIndex);
            loadingScreen.gameObject.SetActive(true);
            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01((operation.progress / .9f));
                loadingBarUI.fillAmount = progress;
                yield return null;
            }
            loadingScreen.gameObject.SetActive(false);
        }
        public void NextScene()
        {
            sceneNum++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
            LevelCompleteUI(false);
        }
        public void PreviousScene()
        {
            sceneNum--;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
        }
        #endregion //==================================================================================================

        private void ResetCollectibleN()
        {
            collectible = 0;
            total = 0;
            sumCollected = 0;
        }
        private void ButtonSetup(Button[] buttons)
        {
            foreach (var button in buttons)
            {
                if (buttons == restartButtons)
                {
                    button.onClick.AddListener(Restart);
                }
                if (buttons == mainMenuButtons)
                {
                    button.onClick.AddListener(ToMenu);
                }
                if (buttons == quitButtons)
                {
                    button.onClick.AddListener(QuitGame);
                }
            }
        }
        private static string TimeFormatter( float seconds )
        {
            var secondsRemainder = Mathf.Floor( (seconds % 60) * 100) / 100.0f;
            var minutes = ((int)(seconds / 60)) % 60;
            var hours = (int)(seconds / 3600);
            return hours == 0 ? $"Total play time | {minutes:00} Min : {secondsRemainder:00} Sec" :
                $"Total play time | {hours} hr : {minutes:00} Min : {secondsRemainder:00} Sec";
        }
    }
}
