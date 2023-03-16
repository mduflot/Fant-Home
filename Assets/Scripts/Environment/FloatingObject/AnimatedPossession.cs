using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedPossession : MonoBehaviour
{
    [SerializeField] Animator animator;
    int ghostInStack = 0;
    [SerializeField]
    private LayerMask enemies;
    [SerializeField]
    private string[] sounds;
    [SerializeField]
    private float maxEmitDistance = 20f;
    [SerializeField]
    private bool loopedSound = false;
    [SerializeField]
    private bool soundOnAnimationStart = false;
    private Coroutine loop;
    PlayerSpawnManager playerManager;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("PlayerManager") != null)
        playerManager = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerSpawnManager>();
        Transform objectToMove = null;
        if (transform.parent != null)
        {
            objectToMove = transform.parent;
            transform.parent = objectToMove.parent;
            objectToMove.parent = transform.GetChild(0).transform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if(ghostInStack == 0)
            {
                OnGhostIn();
            }
            ghostInStack++;
            animator.SetInteger("Stack", ghostInStack);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            ghostInStack--;
            animator.SetInteger("Stack", ghostInStack);
            if (ghostInStack <= 0)
            {
                OnNoGhost();
            }
        }
    }

    private void OnGhostIn()
    {
        if (!soundOnAnimationStart)
            PlaySound();
    }

    private void OnNoGhost()
    {
        if (loop != null) StopCoroutine(loop);
    }

    private void PlaySound()
    {
        int soundToPlay = Random.Range(0, sounds.Length);
        if (loopedSound)
        {
            loop = StartCoroutine(
                LoopCoroutine(
                    AudioManager.Instance.PlaySFXRandom(sounds[soundToPlay], 0.8f, 1.2f, CalculateLevelX())));
        }
        else
        {
            AudioManager.Instance.PlaySFXRandom(sounds[soundToPlay], 0.8f, 1.2f, CalculateLevelX());
        }
    }

    private IEnumerator LoopCoroutine(float length)
    {
        yield return new WaitForSeconds(length);
        int soundToPlay = Random.Range(0, sounds.Length);
        if (!soundOnAnimationStart)
            loop = StartCoroutine(
                LoopCoroutine(
                    AudioManager.Instance.PlaySFXRandom(sounds[soundToPlay], 0.8f, 1.2f, CalculateLevelX())));
   }

    private float CalculateLevelX()
    {
        float dist = Mathf.Infinity;
        float levelX = 1f;
        foreach (GameObject player in playerManager.playersList)
        {
            float currentDist = Vector3.Distance(player.transform.position, transform.position);
            if (currentDist < dist)
            {
                dist = currentDist;
            }
        }
        levelX = 1 / dist * maxEmitDistance;
        return levelX;
    }
}
