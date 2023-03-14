using System.Collections;
using Scriptables;
using Unity.Collections;
using UnityEngine;

public class Ghost : MonoBehaviour, IEnemy
{
    [ReadOnly] public Room CurRoom;
    
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

    private Coroutine StunCO;
    private Coroutine VeilCO;
    private Coroutine RegenCO;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet") && _isVulnerable)
        {
            Pooler.instance.Depop(other.gameObject.GetComponent<Bullet>().key, other.gameObject);
            TakeDamage(other.gameObject.GetComponent<Bullet>().Damage);
            StopCoroutine(VeilCD());
            if (_health > 0) StartCoroutine(VeilCD());
        }
    }

    public void TakeVeil(float damageVeil)
    {
        if (_isVulnerable)
        {
            IsStun = true;
            _isVulnerable = true;
            if (VeilCO != null) StopCoroutine(VeilCO);
            if (RegenCO != null) StopCoroutine(RegenCO);
            if (StunCO != null) StopCoroutine(StunCO);
            VeilCO = StartCoroutine(VeilCD());
            StunCO = StartCoroutine(StunDuration());
            return;
        }
        _veil -= damageVeil;
        StopCoroutine(RegenVeil());
        
        if (!(_veil <= 0)) return;
        IsStun = true;
        _isVulnerable = true;
        VeilCO = StartCoroutine(VeilCD());
        StunCO = StartCoroutine(StunDuration());
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
        RegenCO = StartCoroutine(RegenVeil());
    }

    private IEnumerator RegenVeil()
    {
        while (_veil <= _ghostSO.MaxVeil)
        {
            yield return new WaitForSeconds(1);
            _veil += _regenVeilPoints;
            _meshRenderer.material = _veilMaterial;
            _isVulnerable = false;
        }
    }

    public void TakeDamage(float damage)
    {
        if (_health <= 0) return;
        _health -= damage;
        if (_health <= 0)
        {
            Pooler.instance.Depop("Ghost", gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 5);
    }
}