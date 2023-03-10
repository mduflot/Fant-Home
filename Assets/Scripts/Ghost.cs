using UnityEngine;

public class Ghost : MonoBehaviour, Enemy
{
    private int _health;
    private string _name;
    private int _damage;
    private int _speed;

    void Start()
    {
        _health = 1;
        _name = "trash";
        _damage = 1;
        _speed = 5;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Bullet")) return;
        Pooler.instance.Depop("Bullet", collision.gameObject);
        TakeDamage();
    }

    int Enemy.health => _health;

    string Enemy.name => _name;

    int Enemy.damage => _damage;

    int Enemy.speed => _speed;

    public void Attack()
    {
        
    }

    public void TakeDamage()
    {
        _health--;
        if (_health == 0)
        {
            Pooler.instance.Depop("Ghost", this.gameObject);
        }
    }
}