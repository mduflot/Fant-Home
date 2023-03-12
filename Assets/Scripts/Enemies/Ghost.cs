using System.Collections;
using Scriptables;
using UnityEngine;

public class Ghost : MonoBehaviour, IEnemy
{
    [Header("Stats")]
    [SerializeField] private GhostStatsSO _ghostSO;
    [SerializeField] private string _name = "Ghost";

    [HideInInspector] public bool IsStun;

    [Header("Stats in Runtime")]
    [SerializeField] private float _health = 3;
    [SerializeField] private float _veil = 1;
    private float _regenVeilPoints;
    private float _regenVeilCD;
    private float _durationStun;
    private int _damage = 1;
    private int _speed = 5;

    private bool _isVulnerable;

    private void Start()
    {
        gameObject.name = _name;
        _health = _ghostSO.MaxHealth;
        _veil = _ghostSO.MaxVeil;
        _regenVeilPoints = _ghostSO.VeilRegen;
        _regenVeilCD = _ghostSO.VeilRegenCD;
        _durationStun = _ghostSO.StunDuration;

        _isVulnerable = _veil > 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && _isVulnerable)
        {
            Pooler.instance.Depop(collision.gameObject.GetComponent<Bullet>().key, collision.gameObject);
            TakeDamage(collision.gameObject.GetComponent<Bullet>().Damage);
            StopCoroutine(VeilCD());
            if (_health > 0) StartCoroutine(VeilCD());
        }
    }

    public void TakeVeil(float damageVeil)
    {
        if (_veil <= 0)
        {
            StopCoroutine(VeilCD());
            StartCoroutine(VeilCD());
            return;
        }
        _veil -= damageVeil;
        StopCoroutine(RegenVeil());
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

    private IEnumerator VeilCD()
    {
        yield return new WaitForSeconds(_regenVeilCD);
        Debug.Log("Start regenerate veil", gameObject);
        StartCoroutine(RegenVeil());
    }

    private IEnumerator RegenVeil()
    {
        while (_veil <= _ghostSO.MaxVeil)
        {
            yield return new WaitForSeconds(1);
            _veil += _regenVeilPoints;
            _isVulnerable = false;
            Debug.Log($"Veil took: +{_regenVeilPoints} veil points", gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        //if (_health <= 0) return;
        _health -= damage;
        Debug.Log($"Ghost took: -{damage} damage", gameObject);
        if (_health <= 0)
        {
            Debug.Log($"Ghost is dead", gameObject);
            Pooler.instance.Depop("Ghost", gameObject);
        }
    }
}