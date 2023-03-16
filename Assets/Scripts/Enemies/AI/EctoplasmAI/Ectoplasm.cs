using System;
using System.Collections;
using Scriptables;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Ectoplasm : MonoBehaviour
{
    public enum EctoplasmState
    {
        PATROL,
        POURSUE,
        ATTACKING
    }

    [SerializeField] private Vector3 target;
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private float detectionLength;
    [SerializeField] private LayerMask playerMask, obstaclesMask;
    [SerializeField] private EctoplasmState curState = EctoplasmState.PATROL;

    private bool _reachedTargetPos;
    private Ghost _ghost;
    private GhostStatsSO _stats;

    private int _attackDamage;
    private float _attackRange, _attackCD, _delayBeforeAttack;
    private Vector3 _attackScale;
    private bool _attackIsInCD;
    private string _attackKey;
    private float _isVisibleToPlayer;
    private MeshRenderer _meshRenderer;

    private void Start()
    {
        if (!navAgent) GetComponent<NavMeshAgent>();
        _ghost = GetComponent<Ghost>();
        _stats = _ghost._ghostSO;
        _attackDamage = _stats.AttackDamage;
        _attackRange = _stats.AttackRange;
        _attackScale = _stats.AttackScale;
        _attackCD = _stats.AttackCD;
        _delayBeforeAttack = _stats.AttackDelayBeforeAttack;
        _attackKey = _stats.AttackKey;
        _isVisibleToPlayer = _stats.RangeVisibleToPlayer;
        _meshRenderer = transform.GetComponent<MeshRenderer>();

        SetRandomTarget();
    }

    void Update()
    {
        if (_ghost.IsStun || _ghost.IsAttacking)
        {
            navAgent.isStopped = true;
            return;
        }

        navAgent.isStopped = false;
        CheckNearestPlayer();

        float targetDist = Vector3.Distance(transform.position, target);

        switch (curState)
        {
            case EctoplasmState.PATROL:
                _reachedTargetPos = targetDist <= 0.7f;
                if (navAgent.path.status == NavMeshPathStatus.PathInvalid ||
                    navAgent.path.status == NavMeshPathStatus.PathPartial || _reachedTargetPos) SetRandomTarget();
                break;
            case EctoplasmState.POURSUE:
                if (targetDist < _attackRange && !_attackIsInCD) StartCoroutine(Attack());
                break;
            case EctoplasmState.ATTACKING:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, _isVisibleToPlayer, playerMask);

        if (colliders.Length > 1)
        {
            _meshRenderer.enabled = true;
        }
        else if (_ghost.Veil >= _stats.MaxVeil)
        {
            _meshRenderer.enabled = false;
        }

        if (target != navAgent.destination)
        {
            navAgent.stoppingDistance = _attackRange;
            navAgent.SetDestination(target);
        }
    }

    private void CheckNearestPlayer()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, detectionLength, Vector3.down, detectionLength,
            playerMask);

        bool newTarget = false;
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                if (hit.transform.GetComponent<PlayerHealth>().curHealth <= 0) continue;
                if (EnemyIsVisible(hit.transform))
                {
                    target = hit.transform.position;
                    newTarget = true;
                }
            }

            if (curState != EctoplasmState.ATTACKING && newTarget) curState = EctoplasmState.POURSUE;
            else if (curState != EctoplasmState.ATTACKING) curState = EctoplasmState.PATROL;
        }
    }

    private void SetRandomTarget()
    {
        _reachedTargetPos = false;
        
        Vector3 newPos = transform.position +
                         new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));

        NavMeshHit hit;
        NavMesh.SamplePosition(newPos, out hit, 5, 1);
        target = hit.position;
    }

    IEnumerator Attack()
    {
        curState = EctoplasmState.ATTACKING;
        navAgent.isStopped = true;
        
        transform.LookAt(target);
        GameObject ghost = Pooler.instance.Pop(_attackKey);
        ghost.transform.position = target;
        transform.GetComponent<Ghost>().IsAttacking = true;
        ghost.transform.parent = transform;
        ghost.transform.localPosition = new Vector3(0, 0, 1);
        ghost.transform.localEulerAngles = new Vector3(-180, 180, 0);
        ghost.transform.localScale = new Vector3(1, 1, _attackRange);
        ghost.GetComponent<GhostAttack>().Explode(_attackScale, (target - transform.position), _attackDamage,
            _attackRange, _delayBeforeAttack,
            transform.GetComponent<Ghost>(), _attackKey, _stats.Attack_SFX, _stats.Connect_SFX, navAgent);

        yield return new WaitForSeconds(_delayBeforeAttack);
        curState = EctoplasmState.PATROL;

        _attackIsInCD = true;
        yield return new WaitForSeconds(_attackCD);
        _attackIsInCD = false;
    }

    private bool EnemyIsVisible(Transform trans)
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.Linecast(transform.position, trans.position, out hit, obstaclesMask))
        {
            return false;
        }

        return true;
    }
}