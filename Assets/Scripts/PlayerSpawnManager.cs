using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] private Room initialRoom;
    [SerializeField] private PlayerStatsSO stats;
    [SerializeField] private GameObject PlayerGO;
    [SerializeField] private CinemachineTargetGroup targetGroup;
    [SerializeField] private Transform[] spawnLocations;
    [SerializeField] private Color[] colors;

    private List<GameObject> playersList = new List<GameObject>();

    /*private void Start()
    {
        // for (int i = 0; i < PlayersManager.instance.GetPlayerCount; i++)
        // {
        //     GameObject go = Instantiate(PlayerGO, spawnLocations[i].position, Quaternion.identity);
        //     playersList.Add(go);
        // }
        //
        // PlayersManager.instance.TransferPlayerInput(playersList.ToArray());
        
        //PlayersManager.instance.EnablePlayers(spawnLocations);

        if (PlayersManager.instance != null)
        {
            for (int i = 0; i < PlayersManager.instance.GetPlayerCount; i++)
            {
                PlayerInput.Instantiate(PlayerGO, controlScheme: "Controller", pairWithDevice: Gamepad.all[i]);
            }
        }
        else
        {
            PlayerInput.Instantiate(PlayerGO, controlScheme: "Controller", pairWithDevice: Gamepad.all[PlayerInput.all.Count]);
        }
        
    }*/


    public void SpawnPlayer(PlayerInput playerInput)
    {
        //Debug.Log("PlayerInput ID: " + playerInput.playerIndex);

        GameObject playerGO = playerInput.transform.GetChild(0).gameObject;
        
        targetGroup.AddMember(playerGO.transform, 1,2);
        
        playerInput.gameObject.GetComponent<MeshRenderer>().material.color = colors[playerInput.playerIndex];
        
        playerInput.transform.position = spawnLocations[playerInput.playerIndex].position;
        
        playerInput.gameObject.GetComponent<Player>().curRoom = initialRoom;

        playerInput.gameObject.GetComponent<PlayerHealth>().curHealth = stats.maxHealth;
    }
    
}
