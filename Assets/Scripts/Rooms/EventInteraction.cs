using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventInteraction : Interactible
{
    [SerializeField] private UnityEvent interaction;
    
    public override void Interact()
    {
        interaction?.Invoke();
    }
}
