using UnityEngine;

public class TrashAttack : MonoBehaviour
{
    private Vector3 _center;
    private float _radius;
    private int _damage;
    
    public void Explode(Vector3 center, float radius, int damage)
    {
        GetComponent<ParticleSystem>().Play();
        _center = center;
        _radius = radius;
        _damage = damage;
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
        Pooler.instance.Depop("TrashAttack", gameObject);
    }
}
