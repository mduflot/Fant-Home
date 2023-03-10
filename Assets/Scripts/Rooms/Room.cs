using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private bool locked;
    public bool IsLocked => locked;

    [SerializeField] private FogOfWarTrigger fog;
    [SerializeField] private Door[] doors;
    [SerializeField] private EntryDetector[] entries;
    private List<Player> curPlayersInRoom = new List<Player>();

    private void Awake()
    {
        foreach (var door in doors)
        {
            door.linkedRooms.Add(this);
            door.locked = locked;
        }

        foreach (var entry in entries)
        {
            entry.onTriggerEnterAction += PlayerEnter;
        }
    }

    [ContextMenu("UnlockDoor")]
    public void UnlockDoor()
    {
        locked = false;
        foreach (var door in doors)
        {
            door.locked = false;
        }
    }

    public void PlayerEnter(Player player)
    {
        if (player.curRoom) player.curRoom.PlayerExit(player);
        
        player.curRoom = this;
        
        if(!curPlayersInRoom.Contains(player)) curPlayersInRoom.Add(player);
        
        fog.DisplayRoom();
    }

    private void PlayerExit(Player player)
    {
        curPlayersInRoom.Remove(player);
        CheckFogState();
    }

    private void CheckFogState()
    {
        if(curPlayersInRoom.Count > 0) fog.DisplayRoom();
        else fog.HideRoom();
    }
}
