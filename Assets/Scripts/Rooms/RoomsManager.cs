using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsManager : MonoBehaviour
{
    private Room[] rooms;

    private void Awake()
    {
        rooms = FindObjectsOfType<Room>();
        Debug.Log(rooms.Length + " room(s) found.");
    }
}
