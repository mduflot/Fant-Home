using System.Collections;
using Scriptables;
using Unity.Collections;
using UnityEngine;

public class Ghost : MonoBehaviour, IEnemy
{
    [Header("Stats")]
    public GhostStatsSO _ghostSO;
    [SerializeField] private string _name = "Ghost";
    [SerializeField] private Material _vulnerableMaterial;
    [SerializeField] private Material _stunMaterial;
    [SerializeField] private Material _veilMaterial;

    [HideInInspector] public bool IsStun;

    [Header("Stats in Runtime")]
    [ReadOnly] private float _health = 3;
    [ReadOnly] private float _veil = 1;
    [ReadOnly] private float _regenVeilPoints;
    [ReadOnly] private float _regenVeilCD;
    [ReadOnly] private float _durationStun;
    [ReadOnly] private int _damage = 1;
    [ReadOnly] private int _speed = 5;

    private MeshRenderer _meshRenderer;

    private bool _isVulnerable;

    private void Start()
    {
        gameObject.name = _name;
        _health = _ghostSO.MaxHealth;
        _veil = _ghostSO.MaxVeil;
        _regenVeilPoints = _ghostSO.VeilRegen;
        _regenVeilCD = _ghostSO.VeilRegenCD;
        _durationStun = _ghostSO.StunDuration;
        _meshRenderer = GetComponent<MeshRenderer>();
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
        _meshRenderer.material = _stunMaterial;
        yield return new WaitForSeconds(_durationStun);
        IsStun = false;
        _meshRenderer.material = _vulnerableMaterial;
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
            _meshRenderer.material = _veilMaterial;
            _isVulnerable = false;
            Debug.Log($"Veil took: +{_regenVeilPoints} veil points", gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        if (_health <= 0) return;
        _health -= damage;
        Debug.Log($"Ghost took: -{damage} damage", gameObject);
        if (_health <= 0)
        {
            Debug.Log($"Ghost is dead", gameObject);
            Pooler.instance.Depop("Ghost", gameObject);
        }
    }
}