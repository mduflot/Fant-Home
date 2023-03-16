using System.Collections;
using Entities;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHitable
{
    public enum PlayerState
    {
        BASE,
        DOWN,
        INVINCIBLE
    }

    [SerializeField] public int curHealth;
    [HideInInspector] public float invincibleTime;
    [SerializeField] private PlayerState curState;

    [SerializeField] private GameObject deathInteractionGO;
    [SerializeField] private GameObject _vfxPlayerDead;
    [SerializeField] private ParticleSystem playerElectricity;
    [SerializeField] private Player player;
    private MeshRenderer[] _meshRenderers;
    private float _hitValue;
    private static readonly int Hit = Shader.PropertyToID("_HIT");
    private static readonly int ProgressiveDamage = Shader.PropertyToID("_PROGRESSIVE_DAMAGE");
    
    [SerializeField] private GameObject reanimateUI;
    
    private void Awake()
    {
        curState = PlayerState.BASE;
        _meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
        if (!player) player = GetComponent<Player>();
        reanimateUI.SetActive(false);
    }
    
    [ContextMenu("TakeHits")]
    private void TestHit()
    {
        GetHit(3);
    }

    public void GetHit(int damage)
    {
        _hitValue += 0.05f;
        foreach (var meshRenderer in _meshRenderers)
        {
            meshRenderer.material.SetFloat(Hit, 0.6f);
            meshRenderer.material.SetFloat(ProgressiveDamage, _hitValue);
            StartCoroutine(DamageVFX());
        }
        
        if (curState == PlayerState.INVINCIBLE || curHealth <= 0) return;

        curHealth -= damage;
        var playerElectricityEmission = playerElectricity.emission;
        playerElectricityEmission.rateOverTime = 20 * _hitValue;
        player.playerUI.UpdateHealthUI(curHealth);
        Debug.Log("got hit");
        if (curHealth <= 0)
        {
            Fall();
        }
        else StartCoroutine(Invincible());
        AudioManager.Instance.PlaySFXRandom("Player_Damage", 0.8f, 1.2f);
        
    }

    private IEnumerator Invincible()
    {
        curState = PlayerState.INVINCIBLE;
        yield return new WaitForSeconds(invincibleTime);
        curState = PlayerState.BASE;
    }

    private void Fall()
    {
        curState = PlayerState.DOWN;
        _vfxPlayerDead.SetActive(true);
        _vfxPlayerDead.GetComponent<ParticleSystem>().Play();
        _hitValue = 0;
        foreach (var meshRenderer in _meshRenderers)
        {
            meshRenderer.material.SetFloat(ProgressiveDamage, _hitValue);
        }
        GetComponent<PlayerController>().Immobilisation();
        GetComponent<PlayerController>().enabled = false;
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponentInChildren<PlayerShooter>().enabled = false;
        deathInteractionGO.SetActive(true);
        reanimateUI.SetActive(true);
        GameManager.instance.RemoveFromAliveList();
    }

    public void GetUp()
    {
        curState = PlayerState.BASE;
        curHealth = 3;
        player.playerUI.UpdateHealthUI(curHealth);
        _vfxPlayerDead.SetActive(false);
        _vfxPlayerDead.GetComponent<ParticleSystem>().Stop();
        GetComponent<PlayerController>().enabled = true;
        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponentInChildren<PlayerShooter>().enabled = true;
        deathInteractionGO.SetActive(false);
        AudioManager.Instance.PlaySFXRandom("Player_Revive", 0.8f, 1.2f);
        reanimateUI.SetActive(false);
        GameManager.instance.AddToAliveList();
    }

    private IEnumerator DamageVFX()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (var meshRenderer in _meshRenderers)
        {
            meshRenderer.material.SetFloat(Hit, 0);
        }
    }
}