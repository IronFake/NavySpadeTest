using System;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StartGameView : View
    {
        [SerializeField] private TextMeshProUGUI maxScoreText;
        [SerializeField] private Button playButton;

        public event Action OnPlayButtonPressed;

        private PlayerInfo _playerInfo;
        
        public void Init(PlayerInfo playerInfo)
        {
            _playerInfo = playerInfo;
            playButton.onClick.AddListener(() => {OnPlayButtonPressed?.Invoke();});
        }
        
        public override void Show()
        {
            maxScoreText.text = "Max score: " + _playerInfo.MaxScore;
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}