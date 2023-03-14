using UnityEngine;

public class TrashAttack : MonoBehaviour
{
    public void Explode(Vector3 center, float radius, int damage)
    {
        GetComponent<ParticleSystem>().Play();
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.transform.CompareTag("Player"))
            {
                hitCollider.transform.GetComponent<PlayerHealth>().GetHit(damage);
            }
        }
    }

    private void OnParticleSystemStopped()
    {
        Pooler.instance.Depop("TrashAttack", gameObject);
    }
}
