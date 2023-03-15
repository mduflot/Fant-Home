using BehaviorTree;
using UnityEngine;

namespace AI.PoltergeistAI
{
    public class TaskInteractable : Node
    {
        private Transform _transform;
        private float _attackRange;
        private float _speed;
        
        public TaskInteractable(Transform transform, float attackRange, float speed)
        {
            _transform = transform;
            _attackRange = attackRange;
            _speed = speed;
        }
        
        public override NodeState Evaluate()
        {
            Transform targetObject = (Transform)GetData("object");

            if (targetObject.GetComponent<Interuptor>())
            {
                if (Vector3.Distance(_transform.position, targetObject.position) > _attackRange)
                {
                    _transform.position = Vector3.MoveTowards(
                        _transform.position, targetObject.position, _speed * Time.deltaTime);
                    _transform.LookAt(targetObject.position);
                }
                else
                {
                    ClearData("object");
                    _state = NodeState.FAILURE;
                    return _state;
                }
            }
            else if (targetObject.GetComponent<Door>())
            {
                if (Vector3.Distance(_transform.position, targetObject.position) > _attackRange)
                {
                    _transform.position = Vector3.MoveTowards(
                        _transform.position, targetObject.position, _speed * Time.deltaTime);
                    _transform.LookAt(targetObject.position);
                }
                else
                {
                    ClearData("object");
                    _state = NodeState.FAILURE;
                    return _state;
                }
            }
            else
            {
                if (targetObject.GetComponent<Animation>())
                {
                    targetObject.GetComponent<Animation>().Play();
                }
                else
                {
                    // TODO: Instantiate ParticleSystem on object
                    // Pooler.instance.Pop();
                }
                
                ClearData("object");
                _state = NodeState.SUCCESS;
                return _state;
            }
            
            _state = NodeState.RUNNING;
            return _state;
        }
    }
}