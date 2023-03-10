using System.Collections;
using UnityEngine;

public class GhostManager : MonoBehaviour, IEnemy
{
    // todo: scriptable object for stats
    [SerializeField] private string _name = "Trash";
    [SerializeField] private float _health = 3;
    [SerializeField] private float _veil = 1;
    [SerializeField] private int _durationStun;
    [SerializeField] private int _damage = 1;
    [SerializeField] private int _speed = 5;

    public bool IsStun;

    float IEnemy.health => _health;
    string IEnemy.name => _name;
    float IEnemy.damage => _damage;
    int IEnemy.speed => _speed;

    public void TakeVeil(float damageVeil)
    {
        _veil -= damageVeil;
        if (_veil <= 0)
        {
            IsStun = true;
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