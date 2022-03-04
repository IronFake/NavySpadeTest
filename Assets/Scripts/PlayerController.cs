using System;
using System.Collections;
using DefaultNamespace.Core;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace
{
    public class PlayerController : MonoBehaviour, IPauseable
    {
        [SerializeField] private Camera cam;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        [SerializeField] private LayerMask layerMask;

        private int _runParamHash = Animator.StringToHash("IsRun");
        private Game _game;
        private bool _isPause;
        private float _invincibilityCooldown;
        private WaitForSeconds _waitCooldown;
        private Vector3 _startingPos;
        private Quaternion _startingRotation;

        public bool IsInvincibility { get; private set; }

        public float InvincibilityCooldown => _invincibilityCooldown;
        
        public void Init(float movementSpeed, float invincibilityCooldown, Game game)
        {
            agent.speed = movementSpeed;
            _game = game;
            _invincibilityCooldown = invincibilityCooldown;
            _waitCooldown = new WaitForSeconds(_invincibilityCooldown);
            _startingPos = transform.position;
            _startingRotation = transform.rotation;
        }
        
        private void Update()
        {
            if(_isPause)
                return;

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                SetDestinationPoint(Input.mousePosition);
            }
#else
            if (Input.touchCount > 0)
            {
                SetDestinationPoint(Input.GetTouch(0).position);
            }
#endif
            CheckAnimationState();
        }

        private void SetDestinationPoint(Vector2 screenPos)
        {
            Ray ray = cam.ScreenPointToRay(screenPos);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000, layerMask))
            {
                Vector3 point = hit.point;
                point.y = 0;
                agent.SetDestination(point);   
            }
        }

        private void CheckAnimationState()
        {
            if (agent.remainingDistance < agent.stoppingDistance)
            {
                animator.SetBool(_runParamHash, false);
            }
            else
            {
                animator.SetBool(_runParamHash, true);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            CrystalBehaviour crystalBehaviour = other.GetComponent<CrystalBehaviour>();
            if (crystalBehaviour)
            {
                _game.PickUpCrystalByPlayer(crystalBehaviour);
                return;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            EnemyBehaviour enemyBehaviour = other.collider.GetComponent<EnemyBehaviour>();
            if (enemyBehaviour)
            {
                _game.GetDamage(enemyBehaviour);
                return;
            }
        }

        public void SetPause(bool pause)
        {
            _isPause = pause;
            agent.ResetPath();
            CheckAnimationState();
        }

        public void ResetPosition()
        {
            transform.position = _startingPos;
            transform.rotation = _startingRotation;
            agent.SetDestination(transform.position);
        }

        public void SetInvincibility()
        {
            IsInvincibility = true;
            StartCoroutine(SetInvincibilityCooldown());
        }

        IEnumerator SetInvincibilityCooldown()
        {
            yield return _waitCooldown;
            IsInvincibility = false;
        }
    }
}