using AI.GhostAI;
using UnityEngine;

public class Ghost : MonoBehaviour, Enemy
{
    private int _health;
    private string _name;
    private int _damage;
    private float _speed;

    [SerializeField] private GhostSO monster;
    

    void Start()
    {
        LoadType();
    }

    private void LoadType()
    {
        _health = monster.health;
        _name = monster.key.ToString();
        _damage = monster.damage;
        _speed = monster.speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Bullet")) return;
        Pooler.instance.Depop(collision.gameObject.GetComponent<Bullet>().key, collision.gameObject);
        TakeDamage();
    }

    public void Attack()
    {
        
    }

    public void TakeDamage()
    {
        _health--;
        if (_health == 0)
        {
            Pooler.instance.Depop(_name, this.gameObject);
        }
    }
}