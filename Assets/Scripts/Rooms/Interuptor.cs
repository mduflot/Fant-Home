using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interuptor : Interactible
{
    [SerializeField] private Door[] linkedDoor = Array.Empty<Door>();
    [SerializeField] private StaticFlashLight[] linkedLights = Array.Empty<StaticFlashLight>();
    
    [ContextMenu("Interact")]
    public override void Interact(PlayerInteract player)
    {
        foreach (var door in linkedDoor)
        {
            door.ToggleDoor();
        }

        foreach (var light in linkedLights)
        {
            light.ToggleLight();
        }
        AudioManager.Instance.PlaySFXRandom("Lever", 0.8f, 1.2f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (var door in linkedDoor)
        {
            Gizmos.DrawLine(transform.position, door.transform.position);
        }
        foreach (var light in linkedLights)
        {
            Gizmos.DrawLine(transform.position, light.transform.position);
        }
    }
}
