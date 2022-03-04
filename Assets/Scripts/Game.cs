using System;
using UI;
using UnityEngine;

namespace DefaultNamespace.Core
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private LevelConfig levelConfig;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private EnemiesSpawner enemiesSpawner;
        [SerializeField] private CrystalsSpawner crystalsSpawner;
        [SerializeField] private HUDView hudView;
        [SerializeField] private StartGameView startGameView;
        [SerializeField] private EndGameView endGameView;
 
        private PlayerInfo _playerInfo;
        private Saver _saver;
        public bool IsPaused { get; private set; }
        
        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _saver = new Saver();
            _playerInfo = new PlayerInfo(levelConfig.maxLife, _saver.GetSavedMaxScore());
            _playerInfo.OnLivesIsOver += EndGame;
            enemiesSpawner.Init(levelConfig.movementSpeed, levelConfig.maxEnemies, levelConfig.spawnEnemyInterval, this);
            crystalsSpawner.Init(levelConfig.maxCrystals, levelConfig.spawnCrystalInterval);
            playerController.Init(levelConfig.movementSpeed, levelConfig.invincibilityCooldown, this);
            InitUI();
            startGameView.Show();
            SetPause(true);
        }

        private void InitUI()
        {
            hudView.Init(_playerInfo, enemiesSpawner, crystalsSpawner);
            startGameView.Init(_playerInfo);
            startGameView.OnPlayButtonPressed += StartGame;
            endGameView.Init(_playerInfo);
            endGameView.OnRestartButtonPressed += RestartGame;
        }

        private void StartGame()
        {
            crystalsSpawner.StartSpawn();
            enemiesSpawner.StartSpawn();
            startGameView.Hide();
            endGameView.Hide();
            hudView.Show();
            SetPause(false);
        }
        
        private void RestartGame()
        {
            crystalsSpawner.DestroyAll();
            enemiesSpawner.DestroyAll();
            _playerInfo.Reset();
            playerController.ResetPosition();
            StartGame();
        }

        private void EndGame()
        {
            endGameView.Show();
            hudView.Hide();
            SetPause(true);
            _saver.SaveMaxScore(_playerInfo.MaxScore);
        }

        private void Update()
        {
            Vector3 playerPos = playerController.transform.position;
            hudView.UpdateDistanceToEnemy(enemiesSpawner.GetNearestObjectTo(playerPos));
            hudView.UpdateDistanceToCrystal(crystalsSpawner.GetNearestObjectTo(playerPos));
        }

        public void PickUpCrystalByPlayer(CrystalBehaviour crystalBehaviour)
        {
            _playerInfo.AddLives(1);
            _playerInfo.AddScore(levelConfig.pointsForCrystal);
            crystalsSpawner.DestroyCrystal(crystalBehaviour);
        }
        
        public void PickUpCrystalByEnemy(EnemyBehaviour enemyBehaviour, CrystalBehaviour crystalBehaviour)
        {
            enemiesSpawner.DestroyEnemy(enemyBehaviour);
            crystalsSpawner.DestroyCrystal(crystalBehaviour);
        }

        public void GetDamage(EnemyBehaviour enemyBehaviour)
        {
            if(playerController.IsInvincibility)
                return;
            
            _playerInfo.ReduceLives(1);
            enemiesSpawner.DestroyEnemy(enemyBehaviour);
            playerController.SetInvincibility();
            hudView.ShowInvincibilityText(playerController.InvincibilityCooldown);
        }

        private void SetPause(bool pause)
        {
            IsPaused = pause;
            playerController.SetPause(pause);
            enemiesSpawner.SetPause(pause);
            crystalsSpawner.SetPause(pause);
        }
    }
}
