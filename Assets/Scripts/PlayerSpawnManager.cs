using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] private Room initialRoom;
    [SerializeField] private PlayerStatsSO stats;
    [SerializeField] private CinemachineTargetGroup targetGroup;
    [SerializeField] private Transform[] spawnLocations;
    [SerializeField] private Color[] colors;

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        //Debug.Log("PlayerInput ID: " + playerInput.playerIndex);
        
        targetGroup.AddMember(playerInput.transform, 1,2);
        
        playerInput.gameObject.GetComponent<MeshRenderer>().material.color = colors[playerInput.playerIndex];
        
        playerInput.transform.position = spawnLocations[playerInput.playerIndex].position;
        
        playerInput.gameObject.GetComponent<Player>().curRoom = initialRoom;

        playerInput.gameObject.GetComponent<PlayerHealth>().curHealth = stats.maxHealth;
    }
}
