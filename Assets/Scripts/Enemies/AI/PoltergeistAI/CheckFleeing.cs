using BehaviorTree;
using UnityEngine;

namespace AI.PoltergeistAI
{
    public class CheckFleeing : Node
    {
        private Transform _transform;

        public CheckFleeing(Transform transform)
        {
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
            object t = GetData("target");
            if (_transform.GetComponent<Ghost>().IsFleeing && t is not null)
            {
                _state = NodeState.SUCCESS;
                return _state;
            }

            _state = NodeState.FAILURE;
            return _state;
        }
    }
}