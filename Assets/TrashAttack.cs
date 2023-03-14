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
    }

    private void OnParticleSystemStopped()
    {
        Collider[] hitColliders = Physics.OverlapSphere(_center, _radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.transform.CompareTag("Player"))
            {
                hitCollider.transform.GetComponent<PlayerHealth>().GetHit(_damage);
            }
        }
        _indicator.SetActive(false);
        Pooler.instance.Depop("TrashAttack", gameObject);
    }
    
    private IEnumerator PrepareSpell()
    {
        _indicator.SetActive(true);
        yield return new WaitForSeconds(_attackDelayBeforeAttack);
        GetComponent<ParticleSystem>().Play();
    }
}
