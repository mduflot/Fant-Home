using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

public class PlayersManager : MonoBehaviour
{
    public static PlayersManager instance;
    
    public GameObject PlayerGO;
    
    [SerializeField] private List<PlayerInput> inputs = new List<PlayerInput>();

    public int GetPlayerCount => inputs.Count;

    private List<InputDevice> devices = new List<InputDevice>();

    private void Awake()
    {
        if (!instance) instance = this;
        else Destroy(this.gameObject);
        
        DontDestroyOnLoad(this);
    }

    public void PlayerJoin(PlayerInput playerInput)
    {
        if (inputs.Contains(playerInput)) return;
        
        inputs.Add(playerInput);
        playerInput.transform.SetParent(gameObject.transform);
    }

    public void TransferPlayerInput(GameObject[] playersGO)
    {
        for (int i = 0; i < playersGO.Length; i++)
        {
            /*PlayerInput InputToCopy = inputs[i];
            InputToCopy.DeactivateInput();
            Debug.Log(InputToCopy.devices[0]);
            PlayerInput input = playersGO[i].AddComponent<PlayerInput>();
            
            
            input.actions = InputToCopy.actions;
            StartCoroutine(AssignDevice(input));
            Debug.Log(Gamepad.all[0]);
            input.SwitchCurrentActionMap(InputToCopy.currentActionMap.name);
            
            
            Debug.Log(input.currentActionMap);*/
            
            
        }
    }

    public void EnablePlayers(Transform[] pos)
    {
        for (int i = 0; i < inputs.Count; i++)
        {
            inputs[i].transform.position = pos[i].position;
            inputs[i].transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private IEnumerator AssignDevice(PlayerInput input)
    {
        yield return new WaitForSeconds(1f);
        input.SwitchCurrentControlScheme("Controller", Gamepad.all[0]);
    }
    
}
