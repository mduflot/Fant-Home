using BehaviorTree;
using UnityEngine;

namespace AI.GhostAI
{
    public class CheckPlayerInAttackRange : Node
    {
        private Transform _transform;
        private Animator _animator;
        private float _attackRange;
        private float _attackCounter;
        private float _attackTime;

        public CheckPlayerInAttackRange(Transform transform, float attackRange, float attackTime)
        {
            _transform = transform;
            _animator = transform.GetComponent<Animator>();
            _attackRange = attackRange;
            _attackTime = attackTime;
        }

        public override NodeState Evaluate()
        {
            object t = GetData("target");
            if (t == null)
            {
                _state = NodeState.FAILURE;
                return _state;
            }

            Transform target = (Transform)t;
            _attackCounter += Time.deltaTime;
            if (_attackCounter >= _attackTime)
            {
                if (Vector3.Distance(_transform.position, target.position) <= _attackRange)
                {
                    // _animator.SetBool("Attacking", true);
                    // _animator.SetBool("Walking", false);


                    _state = NodeState.SUCCESS;
                    _attackCounter = 0;
                    return _state;
                }
            }

            _state = NodeState.FAILURE;
            return _state;
        }
    }
}