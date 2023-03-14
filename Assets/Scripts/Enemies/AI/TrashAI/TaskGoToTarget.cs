using BehaviorTree;
using Unity.Mathematics;
using UnityEngine;

namespace AI.GhostAI
{
    public class TaskGoToTarget : Node
    {
        private Transform _transform;
        private MeshRenderer _meshRenderer;
        private LayerMask _enemiesMask;
        private PlayerHealth _playerHealth;
        private float _speed;
        private float _attackRange;
        private float _visibleToPlayer;
        private int _maxHealth;
        private float _maxVeil;

        public TaskGoToTarget(Transform transform, MeshRenderer meshRenderer, LayerMask enemiesMask, float speed,
            float attackRange, float visibleToPlayer, int maxHealth,
            float maxVeil)
        {
            _transform = transform;
            _meshRenderer = meshRenderer;
            _enemiesMask = enemiesMask;
            _speed = speed;
            _attackRange = attackRange;
            _visibleToPlayer = visibleToPlayer;
            _maxHealth = maxHealth;
            _maxVeil = maxVeil;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            _playerHealth = target.GetComponent<PlayerHealth>();
            
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
                if (Vector3.Distance(_transform.position, target.position) > _attackRange)
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

            Vector3 posToGo = target.position +
                              ((_transform.position - hits[currentIndex].transform.position).normalized * 2);
            Vector3 posToGoCheck = new Vector3(posToGo.x, target.position.y, posToGo.z);

            if (Vector3.Distance(_transform.position, target.position) < _visibleToPlayer)
            {
                _meshRenderer.enabled = true;
            }
            else if (_playerHealth.curHealth >= _maxHealth || _transform.GetComponent<Ghost>().Veil >= _maxVeil)
            {
                _meshRenderer.enabled = false;
            }

            if (Vector3.Distance(_transform.position, target.position) > _attackRange)
            {
                _transform.position = Vector3.MoveTowards(
                    _transform.position, posToGoCheck, _speed * Time.deltaTime);
                _transform.LookAt(target.position);
            }

            _state = NodeState.RUNNING;
            return _state;
        }
    }
}