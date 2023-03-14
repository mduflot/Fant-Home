using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public Room curRoom;
    [SerializeField] public PlayerShooter playerShoot;
    [SerializeField] public FlashLight flashLight;

    

    private void Start()
    {
        curRoom?.PlayerEnter(this);
    }
}
