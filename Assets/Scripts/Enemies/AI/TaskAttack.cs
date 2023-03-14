using BehaviorTree;
using UnityEngine;

namespace AI.GhostAI
{
    public class TaskAttack : Node
    {
        private Animator _animator;
        private Transform _transform;
        private Transform _lastTarget;
        private PlayerHealth _playerHealth;
        private int _damage;
        private Vector3 _attackScale;
        private string _attackKey;
        private float _attackDelayBeforeAttack;

        public TaskAttack(Transform transform, int damage, Vector3 attackScale, string attackKey, float attackDelayBeforeAttack)
        {
            _transform = transform;
            _animator = transform.GetComponent<Animator>();
            _damage = damage;
            _attackScale = attackScale;
            _attackKey = attackKey;
            _attackDelayBeforeAttack = attackDelayBeforeAttack;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            if (target != _lastTarget)
            {
                _playerHealth = target.GetComponent<PlayerHealth>();
                _lastTarget = target;
            }

            if (_playerHealth.curHealth <= 0)
            {
                ClearData("target");
                // _animator.SetBool("Attacking", false);
                // _animator.SetBool("Walking", true);
            }
            else
            {
                GameObject attackTrash = Pooler.instance.Pop(_attackKey);
                attackTrash.transform.position = target.position;
                attackTrash.GetComponent<TrashAttack>().Explode(_transform.position, _attackScale, _damage, _attackDelayBeforeAttack);
                _transform.GetComponent<Ghost>().IsFleeing = true;
            }

            _state = NodeState.SUCCESS;
            return _state;
        }
    }
}