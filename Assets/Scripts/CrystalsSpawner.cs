using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utils;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class CrystalsSpawner : MonoBehaviour, IObjectsHolder, IPauseable
    {
        [SerializeField] private CrystalBehaviour crystalPrefab;
        [SerializeField] private Transform parent;
        [SerializeField] private float yOffset;
        [SerializeField] private float spawnRadius;
        
        private List<CrystalBehaviour> _crystalList = new List<CrystalBehaviour>();
        
        public event Action<int> OnObjectCountChanged;
        public int Count => _crystalList.Count;

        private int _maxCrystals;
        private float _spawnInterval;
        private float _currentTime;
        private bool _canSpawn;
        
        public void Init(int maxCrystals, float spawnInterval)
        {
            _maxCrystals = maxCrystals;
            _spawnInterval = spawnInterval;
        }
        
        public void StartSpawn()
        {
            for (int i = 0; i < _maxCrystals; i++)
            {
                SpawnCrystal();
            }

            _canSpawn = true;
        }
        
        private void SpawnCrystal()
        {
            if (NavMeshUtil.TryGetRandomPoint(Vector3.zero, spawnRadius, out Vector3 point))
            {
                point.y = yOffset;
                var crystal = Instantiate(crystalPrefab, point, Quaternion.identity, parent);
                _crystalList.Add(crystal);
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
                if (_crystalList.Count < _maxCrystals)
                {
                    SpawnCrystal();
                    _spawnInterval = Random.Range(1, _spawnInterval);
                }
                _currentTime = 0;
            }
        }

        public void DestroyCrystal(CrystalBehaviour crystalBehaviour)
        {
            if (_crystalList.Contains(crystalBehaviour))
            {
                Destroy(crystalBehaviour.gameObject);
                _crystalList.Remove(crystalBehaviour);
                OnObjectCountChanged?.Invoke(Count);
            }
        }
        
        public float GetNearestObjectTo(Vector3 pos)
        {
            var allEnemies = _crystalList;
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(Vector3.zero, spawnRadius);
        }

        public void SetPause(bool pause)
        {
            _canSpawn = !pause;
        }

        public void DestroyAll()
        {
            for (int i = 0; i < _crystalList.Count; i++)
            {
                Destroy(_crystalList[i].gameObject);
            }
            _crystalList.Clear();
        }
    }
}