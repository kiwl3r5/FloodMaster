using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Manager
{
    public class ScoreManager : MonoBehaviour
    {
        public GameManager gameManager;
        
        [SerializeField] private float maxTimeScore = 5000;
        [SerializeField] private float timeScoreDuration = 360;
        public TMPro.TextMeshProUGUI myName;
        private float _defaultTimeScoreDuration;
        private float _defaultMaxTimeScore;
        //public float rawScore;
        public Text bestScoreText;
        public int calScore;
        [SerializeField] private HighScores _highScores;
        
        private static ScoreManager _instance;
        public static ScoreManager Instance { get { return _instance; } }

        private void Awake()
        {
            _instance = this;
            _highScores = GetComponent<HighScores>();
            gameManager = GetComponent<GameManager>();
            _defaultTimeScoreDuration = timeScoreDuration;
            _defaultMaxTimeScore = maxTimeScore;
        }

        private void Update()
        {
            if (gameManager.sceneNum!=0)
            {
                TimeScoreCounter();
                bestScoreText.text = $"{PlayerPrefs.GetInt("highScore")} POINTS";
                calScore = (int) ScoreCalculator();
            }
        }

        private void TimeScoreCounter()
        {
            if (maxTimeScore <= 0)
            {
                maxTimeScore = 0;
                return;
            }
            timeScoreDuration -= Time.deltaTime;
            var timeScale = timeScoreDuration / _defaultTimeScoreDuration;
            maxTimeScore = _defaultMaxTimeScore*timeScale;
        }

        public void ResetTimeScore()
        {
            maxTimeScore = _defaultMaxTimeScore;
            timeScoreDuration = _defaultTimeScoreDuration;
        }

        public float ScoreCalculator()
        {
            var outScore = GameManager.Instance.rawScore + maxTimeScore;
            return outScore;
        }

        public void SendScore()
        {
            GameManager.Instance.scoreText.text = $"{calScore} points";
            if (calScore > PlayerPrefs.GetInt("highScore"))
            {
                PlayerPrefs.SetInt("highScore", calScore);
                _highScores.UploadScore(myName.text, calScore);
            }
        }
    }
}