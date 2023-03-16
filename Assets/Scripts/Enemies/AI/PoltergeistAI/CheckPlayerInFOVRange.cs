using BehaviorTree;
using UnityEngine;

namespace AI.PoltergeistAI
{
    public class CheckPlayerInFOVRange : Node
    {
        private Transform _transform;
        private Animator _animator;
        private float _fovRange;

        public CheckPlayerInFOVRange(Transform transform, float fovRange)
        {
            _transform = transform;
            _animator = transform.GetComponent<Animator>();
            _fovRange = fovRange;
        }

        public override NodeState Evaluate()
        {
            Collider[] colliders = Physics.OverlapSphere(
                _transform.position, _fovRange);

            if (colliders.Length > 0)
            {
                foreach (var collider in colliders)
                {
                    if (!collider.gameObject.CompareTag("Player")) continue;
                    Parent.Parent.SetData("target", collider.transform);
                    // _animator.SetBool("Walking", true);
                    _state = NodeState.SUCCESS;
                    return _state;
                }
            }

            ClearData("target");
            _state = NodeState.FAILURE;
            return _state;
        }
    }
}