using System.Collections;
using UnityEngine;

public class GhostManager : MonoBehaviour, IEnemy
{
    // todo: scriptable object for stats
    [SerializeField] private string _name = "Trash";
    [SerializeField] private int _health = 3;
    [SerializeField] private int _veil = 1;
    [SerializeField] private int _durationStun;
    [SerializeField] private int _damage = 1;
    [SerializeField] private int _speed = 5;

    public bool IsStun;

    int IEnemy.health => _health;
    string IEnemy.name => _name;
    int IEnemy.damage => _damage;
    int IEnemy.speed => _speed;

    public void TakeVeil(int damageVeil)
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

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Pooler.instance.Depop("Ghost", gameObject);
        }
    }
}