using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public Room curRoom;

    

    private void Start()
    {
        curRoom?.PlayerEnter(this);
    }
}
