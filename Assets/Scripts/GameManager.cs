using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public WaveTool waveTool;
    public RoomsManager roomsManager;
    public PlayerSpawnManager playerManager;

    private void Awake()
    {
        if(instance) Destroy(gameObject);
        else
        {
            instance = this;
        }

        if (waveTool == null) waveTool = FindObjectOfType<WaveTool>();
        if (roomsManager == null) roomsManager = FindObjectOfType<RoomsManager>();
        if (playerManager == null) playerManager = FindObjectOfType<PlayerSpawnManager>();
    }
}
