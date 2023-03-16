using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public float damage;
    [HideInInspector]
    public string key;
    [HideInInspector] 
    public float AOE_Range;

    private float _currentSpeed;
    private delegate void BulletBehaviour();
    private BulletBehaviour _behaviour;

    private delegate void BulletContact();
    private BulletContact _contact;

    private void OnEnable()
    {
        _currentSpeed = speed;
    }

    private void OnDisable()
    {
        _behaviour = () => { };
        _contact = () => { };
    }

    void Update()
    {
        _behaviour();
    }

    public void Contact()
    {
        _contact();
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        _contact();
    }

    public void SetBase()
    {
        _behaviour += BaseBehaviour;
        _contact += BaseContact;
    }

    public void SetExplosive()
    {
        SetBase();
        _behaviour += ExplosiveBehaviour;
        //_contact -= BaseContact;
        _contact += ExplosiveContact;
    }

    public void SetMultiple()
    {
        SetBase();
        _behaviour += MultipleBehaviour;
        _contact -= BaseContact;
        _contact += MultipleContact;
    }

    private void BaseBehaviour()
    {
        transform.position += transform.forward * (Time.deltaTime * _currentSpeed);
    }

    private void BaseContact()
    {
        Pooler.instance.Depop(key, gameObject);
        GameObject particles = Pooler.instance.Pop("VFX_"+key+"Hit");
        particles.transform.position = transform.position;
        Pooler.instance.DelayedDepop(0.5f, "VFX_"+key+"Hit", particles);
    }

    private void ExplosiveBehaviour()
    {
        _currentSpeed = _currentSpeed + 0.05f;
    }

    private void ExplosiveContact()
    {
        Collider [] enemies = Physics.OverlapSphere(transform.position, AOE_Range, 1 << 7 , QueryTriggerInteraction.Collide);
        
        foreach (var enemy in enemies)
        {
            enemy.gameObject.GetComponent<Ghost>().TakeDamage(damage);
        }
    }
    
    private void MultipleBehaviour()
    {
        _currentSpeed = _currentSpeed - 0.05f;
        if (_currentSpeed <= 0)
        {
            Pooler.instance.Depop(key, gameObject);
        }
    }

    private void MultipleContact()
    {
        GameObject particles = Pooler.instance.Pop("VFX_"+key+"Hit");
        particles.transform.position = transform.position;
        Pooler.instance.DelayedDepop(0.5f, "VFX_"+key+"Hit", particles);
    }
}
