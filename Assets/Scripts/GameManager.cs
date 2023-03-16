using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public WaveTool waveTool;
    public RoomsManager roomsManager;
    public PlayerSpawnManager playerManager;
    public MessageDisplayer messageDisplayer;
    public InGameUIManager inGameUiManager;

    private int _alivePlayers;
    public bool isLastWave;
    public bool started;
    private bool _isOver;
    
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
        if (messageDisplayer == null) messageDisplayer = FindObjectOfType<MessageDisplayer>();
        if (inGameUiManager == null) inGameUiManager = FindObjectOfType<InGameUIManager>();
    }

    private void Update()
    {
        if (_isOver || !started) return;
        CheckAlivePlayers();
    }

    private void CheckAlivePlayers()
    {
        if (_alivePlayers <= 0 && !_isOver && playerManager.playersList.Count > 0)
        {
            Debug.Log("perdu");
            _isOver = true;
        }
    }

    public void AddToAliveList()
    {
        _alivePlayers++;
    }
    
    public void RemoveFromAliveList()
    {
        _alivePlayers--;
    }
    
    public void CheckWin()
    {
        
        if (isLastWave)
        {
            Debug.Log("Victory");
        }
    }
}
