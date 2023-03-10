using UnityEngine;
using BehaviorTree;

namespace GhostAI
{
    public class TaskGoToTarget : Node
    {
        private Transform _transform;

        public TaskGoToTarget(Transform transform)
        {
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");

            if (Vector3.Distance(_transform.position, target.position) > 0.01f)
            {
                _transform.position = Vector3.MoveTowards(
                    _transform.position, target.position, GhostBT.Speed * Time.deltaTime);
                _transform.LookAt(target.position);
            }

            _state = NodeState.RUNNING;
            return _state;
        }
    }
}