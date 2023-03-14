using BehaviorTree;
using Unity.Mathematics;
using UnityEngine;

namespace AI.GhostAI
{
    public class TaskGoToTarget : Node
    {
        private Transform _transform;
        private LayerMask _enemiesMask;

        private float _speed;
        private float _attackRange;

        public TaskGoToTarget(Transform transform, LayerMask enemiesMask, float speed, float attackRange)
        {
            _transform = transform;
            _enemiesMask = enemiesMask;
            _speed = speed;
            _attackRange = attackRange;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");

            RaycastHit[] hits = Physics.SphereCastAll(_transform.position, 5f, _transform.forward, 5f, _enemiesMask);
            bool isEnemy = false;

            foreach (var hit in hits)
            {
                if (hit.transform.CompareTag("Enemy") && _transform != hit.transform)
                {
                    isEnemy = true;
                }
            }

            if (!isEnemy)
            {
                if (Vector3.Distance(_transform.position, target.position) > 0.01f)
                {
                    _transform.position = Vector3.MoveTowards(
                        _transform.position, target.position, _speed * Time.deltaTime);
                    _transform.LookAt(target.position);
                }

                _state = NodeState.RUNNING;
                return _state;
            }

            int currentIndex = 0;
            float previousDistance = 0;

            for (var index = 0; index < hits.Length; index++)
            {
                var hit = hits[index];
                if (hits[index].transform == _transform) continue;
                float currentDistance = math.sqrt(math.lengthsq(_transform.position - hit.transform.position));
                if (currentDistance < previousDistance || previousDistance == 0)
                {
                    previousDistance = currentDistance;
                    currentIndex = index;
                }
            }

            Vector3 transformPos = new Vector3(_transform.position.x, target.position.y, _transform.position.z);

            if (Vector3.Distance(_transform.position, target.position) > _attackRange)
            {
                _transform.position = Vector3.MoveTowards(
                    _transform.position,
                    target.position + ((transformPos - hits[currentIndex].transform.position).normalized * 2),
                    _speed * Time.deltaTime);
                _transform.LookAt(target.position);
            }

            _state = NodeState.RUNNING;
            return _state;
        }
    }
}