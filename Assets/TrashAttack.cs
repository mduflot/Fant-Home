using System;
using System.Collections;
using UnityEngine;

public class TrashAttack : MonoBehaviour
{
    [SerializeField] private GameObject _indicator;
    
    private Vector3 _center;
    private float _radius;
    private int _damage;
    private float _attackDelayBeforeAttack;
    
    public void Explode(Vector3 center, float radius, int damage, float attackDelayBeforeAttack)
    {
        _center = center;
        _radius = radius;
        _damage = damage;
        _attackDelayBeforeAttack = attackDelayBeforeAttack;

        StartCoroutine(PrepareSpell());
        AudioManager.Instance.PlaySFXRandom("Ghost_Trash_Attack", 0.8f, 1.2f);
    }

    private void OnParticleSystemStopped()
    {
        Collider[] hitColliders = Physics.OverlapSphere(_center, _radius);
        bool hitAPlayer = false;
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.transform.CompareTag("Player"))
            {
                hitCollider.transform.GetComponent<PlayerHealth>().GetHit(_damage);
                AudioManager.Instance.PlaySFXRandom("Ghost_Trash_Attack_Connect", 0.8f, 1.2f);
                hitAPlayer = true;
            }
        }
        _indicator.SetActive(false);
        Pooler.instance.Depop("TrashAttack", gameObject);
        if (hitAPlayer)
            AudioManager.Instance.PlaySFXRandom("Ghost_Attack_Whiff", 0.8f, 1.2f);

    }
    
    private IEnumerator PrepareSpell()
    {
        _indicator.SetActive(true);
        yield return new WaitForSeconds(_attackDelayBeforeAttack);
        GetComponent<ParticleSystem>().Play();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
