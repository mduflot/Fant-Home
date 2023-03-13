using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interuptor : Interactible
{
    [SerializeField] private Door[] linkedDoor = Array.Empty<Door>();
    
    [ContextMenu("Interact")]
    public override void Interact(PlayerInteract player)
    {
        foreach (var door in linkedDoor)
        {
            door.ToggleDoor();
        }
    }

    private void OnDrawGizmos()
    {
        foreach (var door in linkedDoor)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, door.transform.position);
        }
    }
}
