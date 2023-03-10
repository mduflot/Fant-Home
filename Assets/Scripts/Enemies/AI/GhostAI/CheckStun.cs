using BehaviorTree;
using UnityEngine;

namespace AI.GhostAI
{
    public class CheckStun : Node
    {
        private Transform _transform;
        
        public CheckStun(Transform transform)
        {
            _transform = transform;
        }
        
        public override NodeState Evaluate()
        {
            if (_transform.GetComponent<Ghost>().IsStun)
            {
                _state = NodeState.RUNNING;
                return _state;
            }
            else
            {
                _state = NodeState.FAILURE;
                return _state;
            }
        }
    }
}