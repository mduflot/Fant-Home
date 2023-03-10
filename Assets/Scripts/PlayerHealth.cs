using System.Collections;
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
    [SerializeField] private PlayerState curState;

    [SerializeField] private GameObject deathInteractionGO;

    private void Awake()
    {
        curState = PlayerState.BASE;
    }
    
    [ContextMenu("TakeHits")]
    private void TestHit()
    {
        GetHit(3);
    }

    public void GetHit(int damage)
    {
        if (curState == PlayerState.INVINCIBLE) return;

        curHealth -= damage;
        Debug.Log("OOF - Health:" + curHealth);
        if (curHealth <= 0) Fall();
        else StartCoroutine(Invincible());
    }

    private IEnumerator Invincible()
    {
        curState = PlayerState.INVINCIBLE;
        yield return new WaitForSeconds(1);
        curState = PlayerState.BASE;
    }

    private void Fall()
    {
        curState = PlayerState.DOWN;
        GetComponent<PlayerController>().enabled = false;
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
        deathInteractionGO.SetActive(true);
    }

    public void GetUp()
    {
        curState = PlayerState.BASE;
        GetComponent<PlayerController>().enabled = true;
        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().useGravity = true;
        deathInteractionGO.SetActive(false);
    }
}