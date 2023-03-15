using System.Linq;
using BehaviorTree;
using Unity.Mathematics;
using UnityEngine;

namespace AI.PoltergeistAI
{
    public class CheckInteractable : Node
    {
        private Transform _transform;
        private float _interactableCounter;
        private float _interactableCD;
        private float _radius;

        public CheckInteractable(Transform transform, float interactableCD, float radius)
        {
            _transform = transform;
            _interactableCD = interactableCD;
            _radius = radius;
        }

        public override NodeState Evaluate()
        {
            object isInteract = GetData("isInteract");

            if (isInteract != null)
            {
                _state = NodeState.SUCCESS;
                return _state;
            }

            _interactableCounter += Time.deltaTime;
            if (_interactableCounter >= _interactableCD)
            {
                int currentIndex = 0;
                float previousDistance = 0;

                Collider[] hitColliders = Physics.OverlapSphere(_transform.position, _radius);

                if (hitColliders.Length < 1)
                {
                    _state = NodeState.FAILURE;
                    return _state;
                }

                if (!hitColliders.Any(hitCollider =>
                        hitCollider.CompareTag("Interactable") || hitCollider.CompareTag("Wall")))
                {
                    Debug.Log("No object detect");
                    _state = NodeState.FAILURE;
                    return _state;
                }

                Debug.Log("object detected");
                for (var index = 0; index < hitColliders.Length; index++)
                {
                    var hitCollider = hitColliders[index];
                    if (!hitCollider.transform.CompareTag("Interactable") &&
                        !hitCollider.transform.CompareTag("Wall")) continue;
                    float currentDistance =
                        math.sqrt(math.lengthsq(_transform.position - hitCollider.transform.position));
                    if (!(currentDistance < previousDistance) && previousDistance != 0) continue;
                    previousDistance = currentDistance;
                    currentIndex = index;
                }

                _interactableCounter = 0;
                Parent.Parent.SetData("object", hitColliders[currentIndex].transform);
                _state = NodeState.SUCCESS;
                return _state;
            }

            _state = NodeState.FAILURE;
            return _state;
        }
    }
}