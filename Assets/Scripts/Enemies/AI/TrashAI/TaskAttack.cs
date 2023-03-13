using System.Collections;
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
        private float _attackTime;
        private float _attackRadius;
        private float _attackCounter;
        private string _attackKey;

        public TaskAttack(Transform transform, int damage, float attackTime, float attackRadius, string attackKey)
        {
            _transform = transform;
            _animator = transform.GetComponent<Animator>();
            _damage = damage;
            _attackTime = attackTime;
            _attackRadius = attackRadius;
            _attackKey = attackKey;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            if (target != _lastTarget)
            {
                _playerHealth = target.GetComponent<PlayerHealth>();
                _lastTarget = target;
            }

            _attackCounter += Time.deltaTime;
            if (_attackCounter >= _attackTime)
            {
                if (_playerHealth.curHealth <= 0)
                {
                    ClearData("target");
                    // _animator.SetBool("Attacking", false);
                    // _animator.SetBool("Walking", true);
                }
                else
                {
                    GameObject ghost = Pooler.instance.Pop(_attackKey);
                    ghost.transform.position = target.position;
                    ghost.GetComponent<TrashAttack>().Explode(_transform.position, _attackRadius, _damage);
                    _attackCounter = 0f;
                }
            }

            _state = NodeState.SUCCESS;
            return _state;
        }
    }
}