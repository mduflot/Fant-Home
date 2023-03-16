using BehaviorTree;
using UnityEngine;

namespace AI.GhostAI
{
    public class TaskAttack : Node
    {
        private Animator _animator;
        private Transform _transform;
        private PlayerHealth _playerHealth;
        private int _damage;
        private Vector3 _attackScale;
        private string _attackKey;
        private float _attackDelayBeforeAttack;
        private float _attackRange;
        private LayerMask _playerMask;
        private string _attackSFX, _connectSFX;

        public TaskAttack(Transform transform, int damage, Vector3 attackScale, string attackKey,
            float attackDelayBeforeAttack, float attackRange, LayerMask playerMask, string attackSFX, string connectSFX)
        {
            _transform = transform;
            _animator = transform.GetComponent<Animator>();
            _damage = damage;
            _attackScale = attackScale;
            _attackKey = attackKey;
            _attackDelayBeforeAttack = attackDelayBeforeAttack;
            _attackRange = attackRange;
            _playerMask = playerMask;
            _attackSFX = attackSFX;
            _connectSFX = connectSFX;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");

            if (target.GetComponent<PlayerHealth>().curHealth <= 0)
            {
                ClearData("target");
                // _animator.SetBool("Attacking", false);
                // _animator.SetBool("Walking", true);
            }
            else
            {
                _transform.LookAt(target.position);
                GameObject attackGhost = Pooler.instance.Pop(_attackKey);
                _transform.GetComponent<Ghost>().IsAttacking = true;
                attackGhost.transform.parent = _transform;
                attackGhost.transform.localPosition = new Vector3(0, 0, 1);
                if (_attackKey.Contains("Poltergeist"))
                {
                    attackGhost.transform.localEulerAngles = new Vector3(-90, 90, 0);
                    attackGhost.transform.localScale = new Vector3(_attackRange / 2, 1, 1);
                }
                else if (_attackKey.Contains("Ghost"))
                {
                    attackGhost.transform.localEulerAngles = new Vector3(-180, 180, 0);
                    attackGhost.transform.localScale = new Vector3(1, 1, _attackRange);
                }

                attackGhost.GetComponent<GhostAttack>().Explode(_attackScale,
                    target.position - _transform.position, _damage, _attackRange, _attackDelayBeforeAttack,
                    _transform.GetComponent<Ghost>(), _attackKey, _attackSFX, _connectSFX);

                _transform.GetComponent<Ghost>().IsFleeing = true;
            }

            _state = NodeState.SUCCESS;
            return _state;
        }
    }
}