using System;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Collider))]
    public class SpawnZone : MonoBehaviour
    {
        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        public Vector3 GetRandomPoint()
        {
            return _collider.bounds.RandomPoint();
        }
    }
}