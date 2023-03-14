using BehaviorTree;
using UnityEngine;

namespace AI.GhostAI
{
    public class CheckPlayerInAttackRange : Node
    {
        private Transform _transform;
        private Animator _animator;
        private float _attackRange;

        public CheckPlayerInAttackRange(Transform transform, float attackRange)
        {
            _transform = transform;
            _animator = transform.GetComponent<Animator>();
            _attackRange = attackRange;
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
            if (Vector3.Distance(_transform.position, target.position) <= _attackRange)
            {
                // _animator.SetBool("Attacking", true);
                // _animator.SetBool("Walking", false);

                _state = NodeState.SUCCESS;
                return _state;
            }

            _state = NodeState.FAILURE;
            return _state;
        }
    }
}