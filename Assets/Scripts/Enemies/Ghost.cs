using System.Collections;
using Scriptables;
using UnityEngine;

public class Ghost : MonoBehaviour, IEnemy
{
    [SerializeField] private GhostStatsSO _ghostSO;
    [SerializeField] private string _name = "Ghost";

    [HideInInspector] public bool IsStun;

    private float _health = 3;
    private float _shield = 1;
    private int _durationStun;
    private int _damage = 1;
    private int _speed = 5;

    private bool _isVulnerable;

    private void Start()
    {
        gameObject.name = _name;
        _health = _ghostSO.MaxHealth;
        _shield = _ghostSO.MaxVeil;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && _isVulnerable)
        {
            Pooler.instance.Depop(collision.gameObject.GetComponent<Bullet>().key, collision.gameObject);
            // TODO: TakeDamage from Weapon.Damage
            // TakeDamage();
        }
    }

    public void TakeVeil(float damageVeil)
    {
        _shield -= damageVeil;
        if (_shield <= 0)
        {
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