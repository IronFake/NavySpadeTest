using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Core;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class EnemiesSpawner : MonoBehaviour, IObjectsHolder, IPauseable
    {
        [SerializeField] private Transform spawnZoneContent;
        [SerializeField] private EnemyBehaviour enemyPrefab;
        [SerializeField] private Transform parent;
        
        private SpawnZone[] _spawnZones;
        private List<EnemyBehaviour> _enemyList = new List<EnemyBehaviour>();
        private float _enemySpeed;
        private int _maxEnemies;
        private float _spawnInterval;
        private bool _canSpawn;
        private float _currentTime;
        private Game _game;

        public List<EnemyBehaviour> AllEnemies => _enemyList;
        
        public event Action<int> OnObjectCountChanged;
        public int Count => _enemyList.Count;

        

        public void Init(float enemySpeed, int maxEnemies, float spawnInterval, Game game)
        {
            _enemySpeed = enemySpeed;
            _maxEnemies = maxEnemies;
            _spawnInterval = spawnInterval;
            _spawnZones = spawnZoneContent.GetComponentsInChildren<SpawnZone>();
            _game = game;
        }
        
        public void StartSpawn()
        {
            for (int i = 0; i < _maxEnemies; i++)
            {
                SpawnEnemy();
            }

            _canSpawn = true;
        }
        
        private void SpawnEnemy()
        {
            if (TryGetSpawnPosition(out Vector3 position))
            {
                var enemy = Instantiate(enemyPrefab, position, Quaternion.identity, parent);
                enemy.Init(_enemySpeed, _game);
                _enemyList.Add(enemy);
                OnObjectCountChanged?.Invoke(Count);
            }
        }
        
        private void Update()
        {
            if (_canSpawn == false)
                return;
            
            _currentTime += Time.deltaTime;
            if (_currentTime > _spawnInterval)
            {
                if (_enemyList.Count < _maxEnemies)
                {
                    SpawnEnemy();
                }
                _currentTime = 0;
            }
        }
        
        private bool TryGetSpawnPosition(out Vector3 result)
        {
            SpawnZone spawnZone = _spawnZones[Random.Range(0, _spawnZones.Length)];

            for (int i = 0; i < 30; i++)
            {
                Vector3 randomPoint = spawnZone.GetRandomPoint();
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
                {
                    result = hit.position;
                    return true;
                }
            }
            
            result = Vector3.zero;
            return false;
        }
        
        public void DestroyEnemy(EnemyBehaviour enemyBehaviour)
        {
            if (_enemyList.Contains(enemyBehaviour))
            {
                Destroy(enemyBehaviour.gameObject);
                _enemyList.Remove(enemyBehaviour);
                OnObjectCountChanged?.Invoke(Count);
            }
        }

        public float GetNearestObjectTo(Vector3 pos)
        {
            var allEnemies = AllEnemies;
            if (allEnemies.Count <= 0)
                return 0;
            
            float minDistance = Int32.MaxValue;
            for (int i = 0; i < allEnemies.Count; i++)
            {
                var distance = Vector3.Distance(
                    allEnemies[i].transform.position, pos);
                if (distance < minDistance)
                {
                    minDistance = distance;
                }
            }
            
            return minDistance;
        }

        public void SetPause(bool pause)
        {
            _canSpawn = !pause;
            for (int i = 0; i < _enemyList.Count; i++)
            {
                _enemyList[i].SetPause(pause);
            }
        }
        
        public void DestroyAll()
        {
            for (int i = 0; i < _enemyList.Count; i++)
            {
                Destroy(_enemyList[i].gameObject);
            }
            _enemyList.Clear();
        }
    }
}