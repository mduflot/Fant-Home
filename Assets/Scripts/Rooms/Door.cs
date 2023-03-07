using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Door : MonoBehaviour
{
    public GameObject DoorGO;
    public bool opened, locked;
    [HideInInspector] public List<Room> linkedRooms = new List<Room>();

    private void Start()
    {
        CheckIfLocked();
    }

    public void ToggleDoor()
    {
        if (locked) return;
        
        opened = !opened;
        
        if(DoorGO) DoorGO.SetActive(!opened);
        else transform.GetChild(0).gameObject.SetActive(!opened);
    }

    public void CheckIfLocked()
    {
        foreach (var room in linkedRooms)
        {
            if (room.IsLocked)
            {
                locked = true;
                return;
            }
        }

        Debug.Log("no");
        locked = false;
    }
}
