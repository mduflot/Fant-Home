using UnityEngine;
using BehaviorTree;

namespace GhostAI
{
    public class TaskAttack : Node
    {
        private Animator _animator;

        private Transform _lastTarget;
        // private EnemyManager _enemyManager;

        private float _attackTime = 1f;
        private float _attackCounter = 0f;

        public TaskAttack(Transform transform)
        {
            _animator = transform.GetComponent<Animator>();
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            if (target != _lastTarget)
            {
                // _enemyManager = target.GetComponent<EnemyManager>();
                _lastTarget = target;
            }

            _attackCounter += Time.deltaTime;
            if (_attackCounter >= _attackTime)
            {
                bool enemyIsDead = true; // _enemyManager.TakeHit();
                if (enemyIsDead)
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