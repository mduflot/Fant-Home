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
    [ReadOnly] private float _regenVeilOverTime;
    [ReadOnly] private float _stunTime;
    [ReadOnly] private int _damage = 1;
    [ReadOnly] private int _speed = 5;

    private MeshRenderer _meshRenderer;

    private bool _isVulnerable;
    private float _stunCounter;
    private float _veilCounter;

    private Coroutine _regenCO;

    private void Start()
    {
        gameObject.name = _name;
        _health = _ghostSO.MaxHealth;
        _veil = _ghostSO.MaxVeil;
        _regenVeilPoints = _ghostSO.VeilRegenPoints;
        _regenVeilCD = _ghostSO.VeilRegenCD;
        _regenVeilOverTime = _ghostSO.VeilRegenOverTime;
        _stunTime = _ghostSO.StunDuration;
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (IsStun)
        {
            _stunCounter += Time.deltaTime;
            if (_stunCounter >= _stunTime)
            {
                IsStun = false;
                _meshRenderer.material = _vulnerableMaterial;
            }
        }

        if (!(_veil <= 0)) return;
        _veilCounter += Time.deltaTime;
        if (!(_veilCounter >= _regenVeilCD)) return;
        _regenCO = StartCoroutine(RegenVeil());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Bullet") || !_isVulnerable) return;
        if (_regenCO != null) StopCoroutine(_regenCO);
        _veilCounter = 0;
        Pooler.instance.Depop(other.gameObject.GetComponent<Bullet>().key, other.gameObject);
        TakeDamage(other.gameObject.GetComponent<Bullet>().Damage);
    }

    public void TakeVeil(float damageVeil)
    {
        _veil -= damageVeil;
        if (!(_veil <= 0)) return;

        _veil = 0;
        if (_regenCO != null) StopCoroutine(_regenCO);
        AudioManager.Instance.PlaySFXRandom("Ghost_Revealed", 0.8f, 1.2f);
        Debug.Log("Stuck & Veil");
        _stunCounter = 0;
        _veilCounter = 0;
        IsStun = true;
        _isVulnerable = true;
        _meshRenderer.material = _stunMaterial;
    }

    public void TakeDamage(float damage)
    {
        if (_health <= 0) return;
        _health -= damage;
        if (_health <= 0)
        {
            Pooler.instance.Depop(_ghostSO.Key.ToString(), gameObject);
            AudioManager.Instance.PlaySFXRandom(_ghostSO.Death_SFX, 0.8f, 1.2f);
            return;
        }

        AudioManager.Instance.PlaySFXRandom(_ghostSO.Damage_SFX, 0.8f, 1.2f);
    }

    private IEnumerator RegenVeil()
    {
        while (_veil <= _ghostSO.MaxVeil)
        {
            _veil += _regenVeilPoints;
            _isVulnerable = false;
            _meshRenderer.material = _veilMaterial;
            yield return new WaitForSeconds(_regenVeilOverTime);
        }
    }
}