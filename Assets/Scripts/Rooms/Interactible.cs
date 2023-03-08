using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Interactible : MonoBehaviour
{
    public virtual void Interact() { }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerInteract>()?.isInside(true, this);
    }

    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<PlayerInteract>()?.isInside(false);
    }
}
