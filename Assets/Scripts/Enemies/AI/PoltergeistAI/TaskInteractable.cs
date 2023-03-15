using BehaviorTree;
using UnityEngine;

namespace AI.PoltergeistAI
{
    public class TaskInteractable : Node
    {
        private Transform _transform;
        private float _speed;
        private bool _canInteract;

        public TaskInteractable(Transform transform, float speed, bool canInteract)
        {
            _transform = transform;
            _speed = speed;
            _canInteract = canInteract;
        }

        public override NodeState Evaluate()
        {
            Transform targetObject = (Transform)GetData("object");

            if (_canInteract)
            {
                Debug.Log("Interact with something");
                if (targetObject.GetComponent<Interuptor>())
                {
                    if (Vector3.Distance(_transform.position, targetObject.position) > 1f)
                    {
                        Debug.Log("Interact with interuptor");
                        _transform.position = Vector3.MoveTowards(
                            _transform.position, targetObject.position, _speed * Time.deltaTime);
                        _transform.LookAt(targetObject.position);
                        Parent.Parent.SetData("isInteract", true);
                    }
                    else
                    {
                        targetObject.GetComponent<Interuptor>().Interact(new PlayerInteract());
                        ClearData("object");
                        ClearData("isInteract");
                        _state = NodeState.FAILURE;
                        return _state;
                    }
                }
                else if (targetObject.GetComponent<Door>())
                {
                    if (Vector3.Distance(_transform.position, targetObject.position) > 1f)
                    {
                        Debug.Log("Interact with door");
                        _transform.position = Vector3.MoveTowards(
                            _transform.position, targetObject.position, _speed * Time.deltaTime);
                        _transform.LookAt(targetObject.position);
                        Parent.Parent.SetData("isInteract", true);
                    }
                    else
                    {
                        targetObject.GetComponent<Door>().ToggleDoor();
                        ClearData("object");
                        ClearData("isInteract");
                        _state = NodeState.FAILURE;
                        return _state;
                    }
                }
                else
                {
                    Debug.Log("Interact with something with animation or with particleSystem");
                    if (targetObject.GetComponent<Animation>())
                    {
                        targetObject.GetComponent<Animation>().Play();
                    }
                    else
                    {
                        // TODO: Instantiate ParticleSystem on object
                        // Pooler.instance.Pop();
                        Debug.Log("Pop ParticleSystem");
                    }
                }

                ClearData("object");
                _state = NodeState.SUCCESS;
                return _state;
            }

            Debug.Log("Interact with something with animation or with particleSystem");
            if (targetObject.GetComponent<Animation>())
            {
                targetObject.GetComponent<Animation>().Play();
            }
            else
            {
                // TODO: Instantiate ParticleSystem on object
                // Pooler.instance.Pop();
                Debug.Log("Pop ParticleSystem");
            }

            ClearData("object");
            _state = NodeState.SUCCESS;
            return _state;
        }
    }
}