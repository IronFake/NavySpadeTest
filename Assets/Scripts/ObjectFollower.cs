using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class ObjectFollower : MonoBehaviour
    {
        [SerializeField] private Transform followObject;

        private Vector3 _offset;
        
        private void Start()
        {
            _offset = followObject.position - transform.position;
        }

        private void LateUpdate()
        {
            transform.position = followObject.position - _offset;
        }
    }
}