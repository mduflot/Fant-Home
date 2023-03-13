using BehaviorTree;
using UnityEngine;

namespace AI.GhostAI
{
    public class TaskAttack : Node
    {
        private Animator _animator;
        private Transform _lastTarget;
        private PlayerHealth _playerHealth;
        private float _attackTime = 2f;
        private float _attackCounter;

        public TaskAttack(Transform transform)
        {
            _animator = transform.GetComponent<Animator>();
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            if (target != _lastTarget)
            {
                _playerHealth = target.GetComponent<PlayerHealth>();
                _lastTarget = target;
            }

            _attackCounter += Time.deltaTime;
            if (_attackCounter >= GhostBT.AttackTime)
            {
                _playerHealth.GetHit(1);
                if (_playerHealth.curHealth >= 0)
                {
                    ClearData("target");
                    // _animator.SetBool("Attacking", false);
                    // _animator.SetBool("Walking", true);
                }
                else
                {
                    _attackCounter = 0f;
                }
            }

            _state = NodeState.RUNNING;
            return _state;
        }
    }
}