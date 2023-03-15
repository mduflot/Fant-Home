using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedPossession : MonoBehaviour
{
    [SerializeField] Animator animator;
    int ghostInStack = 0;
    [SerializeField] private LayerMask enemies;
    [SerializeField] private string[] sounds;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if(ghostInStack == 0)
            {
                int soundToPlay = Random.Range(0, sounds.Length);
                AudioManager.Instance.PlaySFXRandom(sounds[soundToPlay], 0.8f, 1.2f);
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
        }
    }
}
