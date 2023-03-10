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

    public bool IsVulnerable;
    public bool IsStun; 
    

    private void OnCollisionEnter(Collision collision)
    {
        // todo: add "Light" tag
        if (collision.gameObject.CompareTag("Light"))
        {
            if (!IsVulnerable) TakeVeil();
        }
        if (collision.gameObject.CompareTag("Bullet") && IsVulnerable)
        {
            Pooler.instance.Depop("Bullet", collision.gameObject);
            TakeDamage();
        }
    }

    int IEnemy.health => _health;
    string IEnemy.name => _name;
    int IEnemy.damage => _damage;
    int IEnemy.speed => _speed;

    public void TakeVeil()
    {
        _veil--;
        if (_veil <= 0)
        {
            IsVulnerable = true;
            IsStun = true;
            
        }
    }

    private IEnumerator StunDuration()
    {
        yield return new WaitForSeconds(_durationStun);
        IsStun = false;
    }

    public void TakeDamage()
    {
        _health--;
        if (_health == 0)
        {
            Pooler.instance.Depop("Ghost", gameObject);
        }
    }
}