using System;
using System.Collections;
using DefaultNamespace;
using DefaultNamespace.Core;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HUDView : View
    {
        [SerializeField] private TextMeshProUGUI livesText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI enemyCountText;
        [SerializeField] private TextMeshProUGUI distanceToEnemyText;
        [SerializeField] private TextMeshProUGUI crystalCountText;
        [SerializeField] private TextMeshProUGUI distanceToCrystalText;
        [SerializeField] private TextMeshProUGUI invincibilityText;

        private IObjectsHolder _enemyObjectsHolder;
        private IObjectsHolder _crystalObjectHolder;
        
        public void Init(PlayerInfo playerInfo, IObjectsHolder enemyObjectsHolder, IObjectsHolder crystalObjectHolder)
        {
            playerInfo.OnLivesChanged += UpdateLivesText;
            playerInfo.OnScoreChanged += UpdateScoreText;
            enemyObjectsHolder.OnObjectCountChanged += UpdateEnemyCount;
            crystalObjectHolder.OnObjectCountChanged += UpdateCrystalCount;

            UpdateLivesText(playerInfo.Lives);
            UpdateScoreText(playerInfo.Score);
            UpdateEnemyCount(enemyObjectsHolder.Count);
            UpdateCrystalCount(crystalObjectHolder.Count);
        }
        
        private void UpdateLivesText(int value)
        {
            livesText.text = value.ToString();
        }

        private void UpdateScoreText(int value)
        {
            scoreText.text = value.ToString();
        }

        private void UpdateEnemyCount(int value)
        {
            enemyCountText.text = value.ToString();
        }
        
        private void UpdateCrystalCount(int value)
        {
            crystalCountText.text = value.ToString();
        }

        public void UpdateDistanceToEnemy(float distance)
        {
            distanceToEnemyText.text = ((int) distance).ToString();
        }
        
        public void UpdateDistanceToCrystal(float distance)
        {
            distanceToCrystalText.text = ((int) distance).ToString();
        }

        public void ShowInvincibilityText(float time)
        {
            if (gameObject.activeSelf)
            {
                StartCoroutine(UpdateInvincibilityText(time));
            }
        }

        IEnumerator UpdateInvincibilityText(float time)
        {
            invincibilityText.gameObject.SetActive(true);
            float currentTime = time;
            while (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                invincibilityText.text = "Invincibility \n " + (int)currentTime;
                yield return null;
            }
            
            invincibilityText.gameObject.SetActive(false);
        }

        public override void Show()
        {
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}