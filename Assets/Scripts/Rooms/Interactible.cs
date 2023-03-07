using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    public virtual void Interact(){}

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerInteract>()?.isInside(true, this);
    }

    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<PlayerInteract>()?.isInside(false);
    }
}
