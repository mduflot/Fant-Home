using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private GameObject interactLogo;
    private bool canInteract;
    private Interactible interact;

    public void isInside(bool detect, Interactible inter = null)
    {
        canInteract = detect;
        interact = inter;
        
        interactLogo.SetActive(detect);
    }
    
    private void OnInteract()
    {
        if (canInteract && interact != null)
        {
            interact.Interact();
        }
    }
}
