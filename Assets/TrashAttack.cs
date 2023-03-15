using System.Collections;
using UnityEngine;

public class TrashAttack : MonoBehaviour
{
    [SerializeField] private GameObject _indicator;
    
    private Vector3 _center;
    private Vector3 _scale;
    private int _damage;
    private float _attackDelayBeforeAttack;
    
    public void Explode(Vector3 center, Vector3 scale, int damage, float attackDelayBeforeAttack)
    {
        _center = center;
        _scale = scale;
        _damage = damage;
        _attackDelayBeforeAttack = attackDelayBeforeAttack;

        StartCoroutine(PrepareSpell());
    }

    private void OnParticleSystemStopped()
    {
        Collider[] hitColliders = Physics.OverlapBox(_center, _scale / 2);
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
        if (hitAPlayer) AudioManager.Instance.PlaySFXRandom("Ghost_Attack_Whiff", 0.8f, 1.2f);

    }
    
    private IEnumerator PrepareSpell()
    {
        _indicator.SetActive(true);
        yield return new WaitForSeconds(_attackDelayBeforeAttack);
        GetComponent<ParticleSystem>().Play();
        AudioManager.Instance.PlaySFXRandom("Ghost_Trash_Attack", 0.8f, 1.2f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, _scale);
    }
}
