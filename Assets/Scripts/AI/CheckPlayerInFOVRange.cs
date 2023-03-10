using AI.GhostAI;
using UnityEngine;
using BehaviorTree;

namespace AI
{
    public class CheckPlayerInFOVRange : Node
    {
        private static int _enemyLayerMask = 1 << 6;

        private Transform _transform;
        private Animator _animator;

        public CheckPlayerInFOVRange(Transform transform)
        {
            _transform = transform;
            _animator = transform.GetComponent<Animator>();
        }

        public override NodeState Evaluate()
        {
            object t = GetData("target");
            if (t == null)
            {
                Collider[] colliders = Physics.OverlapSphere(
                    _transform.position, GhostBT.FOVRange, _enemyLayerMask);

                if (colliders.Length > 0)
                {
                    Parent.Parent.SetData("target", colliders[0].transform);
                    // _animator.SetBool("Walking", true);
                    _state = NodeState.SUCCESS;
                    return _state;
                }

                _state = NodeState.FAILURE;
                return _state;
            }

            _state = NodeState.SUCCESS;
            return _state;
        }
    }
}