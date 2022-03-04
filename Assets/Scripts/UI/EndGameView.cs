using System;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EndGameView : View
    {
        [SerializeField] private TextMeshProUGUI maxScoreText;
        [SerializeField] private TextMeshProUGUI currentScoreText;
        [SerializeField] private Button restartButton;

        public event Action OnRestartButtonPressed;

        private PlayerInfo _playerInfo;
        
        public void Init(PlayerInfo playerInfo)
        {
            _playerInfo = playerInfo;
            restartButton.onClick.AddListener(() => OnRestartButtonPressed?.Invoke());
        }

        public override void Show()
        {
            maxScoreText.text = "Max score: " + _playerInfo.MaxScore;
            currentScoreText.text = "Score: " + _playerInfo.Score;
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}