using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private bool locked;

    public bool IsLocked => locked;

    [SerializeField] private Door[] doors;

    private void Awake()
    {
        foreach (var door in doors)
        {
            door.linkedRooms.Add(this);
            door.locked = locked;
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
}
