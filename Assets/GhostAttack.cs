using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GhostAttack : MonoBehaviour
{
    [SerializeField] private GameObject _indicator;

    private Ghost _sender;
    private Vector3 _scale;
    private Vector3 _direction;
    private float _attackRange;
    private int _damage;
    private float _attackDelayBeforeAttack;
    private string _attackKey;
    private NavMeshAgent _navMesh;
    private bool _playerIsHit;
    private string _attackSFX, _connectSFX;

    public void Explode(Vector3 scale, Vector3 direction, int damage,
        float attackRange, float attackDelayBeforeAttack,
        Ghost sender, string attackKey, string attackSFX, string connectSFX, NavMeshAgent navMesh = null)
    {
        _scale = scale;
        _direction = direction;
        _damage = damage;
        _attackRange = attackRange;
        _attackDelayBeforeAttack = attackDelayBeforeAttack;
        _attackKey = attackKey;
        _sender = sender;
        _navMesh = navMesh;
        _playerIsHit = false;
        _attackSFX = attackSFX;
        _connectSFX = connectSFX;

        _indicator.transform.localScale = scale;
        StartCoroutine(PrepareSpell());
    }

    private void OnParticleCollision(GameObject other)
    {
        if (!other.transform.CompareTag("Player")) return;
        other.transform.GetComponent<PlayerHealth>().GetHit(_damage);
        _playerIsHit = true;
        AudioManager.Instance.PlaySFXRandom(_connectSFX, 0.8f, 1.2f);
    }

    private void OnParticleSystemStopped()
    {
        _sender.IsAttacking = false;
        if (_navMesh) _navMesh.isStopped = false;

        if (_indicator) _indicator.SetActive(false);
        if (!_playerIsHit) AudioManager.Instance.PlaySFXRandom("Ghost_Attack_Whiff", 0.8f, 1.2f);
        Pooler.instance.Depop(_attackKey, gameObject);
    }

    private IEnumerator PrepareSpell()
    {
        if (_indicator) _indicator.SetActive(true);
        yield return new WaitForSeconds(_attackDelayBeforeAttack);
        GetComponent<ParticleSystem>().Play();
        AudioManager.Instance.PlaySFXRandom(_attackSFX, 0.8f, 1.2f);
    }

    private void OnDrawGizmos()
    {
        //Draw a Ray forward from GameObject toward the maximum distance
        Gizmos.DrawRay(transform.position, _direction.normalized * _attackRange);
        //Draw a cube at the maximum distance
        Gizmos.DrawWireCube(transform.position + _direction.normalized * _attackRange, _scale);
    }
}