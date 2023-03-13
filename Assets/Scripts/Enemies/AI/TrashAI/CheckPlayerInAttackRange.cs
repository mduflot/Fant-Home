using BehaviorTree;
using UnityEngine;

namespace AI.GhostAI
{
    public class CheckPlayerInAttackRange : Node
    {
        private Transform _transform;
        private Animator _animator;

        public CheckPlayerInAttackRange(Transform transform)
        {
            _transform = transform;
            _animator = transform.GetComponent<Animator>();
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
            if (Vector3.Distance(_transform.position, target.position) <= GhostBT.AttackRange)
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