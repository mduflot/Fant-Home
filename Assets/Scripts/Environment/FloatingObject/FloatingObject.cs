using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [SerializeField] Animator animator;
    int ghostInStack = 0;
    public LayerMask enemies;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != enemies)
        {
            if(ghostInStack == 0)
            {
                AudioManager.Instance.PlaySFXRandom("Floating_Object", 0.8f, 1.2f);
            }
            ghostInStack++;
            animator.SetInteger("Stack", ghostInStack);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != enemies)
        {
            ghostInStack--;
            animator.SetInteger("Stack", ghostInStack);
        }
    }
}
