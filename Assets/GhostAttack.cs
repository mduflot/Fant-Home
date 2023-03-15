using System.Collections;
using UnityEngine;

public class GhostAttack : MonoBehaviour
{
    [SerializeField] private GameObject _indicator;

    private Ghost _sender;
    private Vector3 _center;
    private Vector3 _scale;
    private Vector3 _direction;
    private Quaternion _rotation;
    private LayerMask _playerMask;
    private int _damage;
    private float _attackDelayBeforeAttack;

    public void Explode(Vector3 center, Vector3 scale, Vector3 direction, int damage, float attackDelayBeforeAttack,
        Ghost sender)
    {
        _center = center;
        _scale = scale;
        _direction = direction;
        _damage = damage;
        _attackDelayBeforeAttack = attackDelayBeforeAttack;
        _sender = sender;

        StartCoroutine(PrepareSpell());
    }

    private void OnParticleSystemStopped()
    {
        RaycastHit[] hits = Physics.BoxCastAll(_center, _scale / 2, _direction, _rotation, _playerMask);
        bool hitAPlayer = false;
        if (hits.Length > 0)
        {
            foreach (var hitCollider in hits)
            {
                hitCollider.transform.GetComponent<PlayerHealth>().GetHit(_damage);
                AudioManager.Instance.PlaySFXRandom("Ghost_Trash_Attack_Connect", 0.8f, 1.2f);
                hitAPlayer = true;
            }
        }

        if (_indicator) _indicator.SetActive(false);
        if (hitAPlayer) AudioManager.Instance.PlaySFXRandom("Ghost_Attack_Whiff", 0.8f, 1.2f);
        Pooler.instance.Depop("TrashAttack", gameObject);
    }

    private IEnumerator PrepareSpell()
    {
        if (_indicator) _indicator.SetActive(true);
        yield return new WaitForSeconds(_attackDelayBeforeAttack);
        GetComponent<ParticleSystem>().Play();
        _sender.IsAttacking = false;
        AudioManager.Instance.PlaySFXRandom("Ghost_Trash_Attack", 0.8f, 1.2f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_center, _scale);
    }
}