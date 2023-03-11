using System.Collections;
using Scriptables;
using UnityEngine;

public class Ghost : MonoBehaviour, IEnemy
{
    [SerializeField] private GhostStatsSO _ghostSO;
    [SerializeField] private string _name = "Ghost";

    [HideInInspector] public bool IsStun;

    [SerializeField] private float _health = 3;
    [SerializeField] private float _veil = 1;
    private float _durationStun;
    private int _damage = 1;
    private int _speed = 5;

    private bool _isVulnerable;

    private void Start()
    {
        gameObject.name = _name;
        _health = _ghostSO.MaxHealth;
        _veil = _ghostSO.MaxVeil;
        _durationStun = _ghostSO.StunDuration;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && _isVulnerable)
        {
            Pooler.instance.Depop(collision.gameObject.GetComponent<Bullet>().key, collision.gameObject);
            // TODO: TakeDamage from Weapon.Damage / Bullet Scriptable Object
            TakeDamage(collision.gameObject.GetComponent<Bullet>().Damage);
        }
    }

    public void TakeVeil(float damageVeil)
    {
        if (_veil <= 0) return;
        _veil -= damageVeil;
        Debug.Log($"Veil took: -{damageVeil} damage", gameObject);
        if (_veil <= 0)
        {
            Debug.Log($"Veil is removed", gameObject);
            IsStun = true;
            _isVulnerable = true;
            StartCoroutine(StunDuration());
        }
    }

    private IEnumerator StunDuration()
    {
        yield return new WaitForSeconds(_durationStun);
        IsStun = false;
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Pooler.instance.Depop("Ghost", gameObject);
        }
    }
}