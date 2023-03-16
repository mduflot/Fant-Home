using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Interactible : MonoBehaviour
{
    private List<PlayerInteract> curPlayers = new List<PlayerInteract>();
    public virtual void Interact(PlayerInteract player) { }
    
    public virtual void PlayerExit(PlayerInteract player){ }

    private void OnTriggerEnter(Collider other)
    {
        PlayerInteract player = other.GetComponent<PlayerInteract>();

        if (!player) return;
        
        player.isInside(true, this);
        if (!curPlayers.Contains(player))
        {
            curPlayers.Add(player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerInteract player = other.GetComponent<PlayerInteract>();
        if (!player) return;
        
        player.isInside(false);
        PlayerExit(player);
        curPlayers.Remove(player);
    }

    private void OnDisable()
    {
        foreach (var player in curPlayers)
        {
            player.isInside(false);
        }
        curPlayers.Clear();
    }
}
