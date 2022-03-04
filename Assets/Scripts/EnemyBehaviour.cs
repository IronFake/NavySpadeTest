using System;
using DefaultNamespace.Core;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
    public class EnemyBehaviour : MonoBehaviour
    {
        [SerializeField] private float walkRadius;
        
        private NavMeshAgent _agent;
        private Animator _animator;
        
        private int _runParamHash = Animator.StringToHash("IsRun");
        private Vector3 _targetPoint;
        private Game _game;
        private bool _isPause;
        
        public void Init(float movementSpeed, Game game)
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _agent.speed = movementSpeed;
            _game = game;
        }
        
        private void Update()
        {
            if(_agent == null)
                return;
            
            if(_isPause)
                return;
            
            if (_agent.remainingDistance < _agent.stoppingDistance)
            {
                _animator.SetBool(_runParamHash, false);
                SetDestinationPoint();
            }
            else
            {
                _animator.SetBool(_runParamHash, true);
            }
        }
        
        private void SetDestinationPoint()
        {
            for (int i = 0; i < 30; i++)
            {
                Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
                randomDirection += transform.position;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomDirection, out hit, walkRadius, NavMesh.AllAreas))
                {
                    _targetPoint = hit.position;
                    _agent.SetDestination(_targetPoint);
                    return;
                }
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            CrystalBehaviour crystalBehaviour = other.GetComponent<CrystalBehaviour>();
            if (crystalBehaviour)
            {
                _game.PickUpCrystalByEnemy(this, crystalBehaviour);
                return;
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, walkRadius);
        }

        public void SetPause(bool pause)
        {
            _isPause = pause;
            _agent.isStopped = pause;
            _animator.SetBool(_runParamHash, false);
        }
    }
}