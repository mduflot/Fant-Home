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
    private bool loopedSound = false;
    private Coroutine loop;

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
                    AudioManager.Instance.PlaySFXRandom(sounds[soundToPlay], 0.8f, 1.2f)));
        }
        else
        {
            AudioManager.Instance.PlaySFXRandom(sounds[soundToPlay], 0.8f, 1.2f);
        }
    }

    private IEnumerator LoopCoroutine(float length)
    {
        yield return new WaitForSeconds(length);
        int soundToPlay = Random.Range(0, sounds.Length);
        loop = StartCoroutine(
            LoopCoroutine(
                AudioManager.Instance.PlaySFXRandom(sounds[soundToPlay], 0.8f, 1.2f)));
   }
}
