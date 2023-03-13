using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayersDevice", menuName = "Scriptables/PlayersDevice", order = 1 )]
public class PlayersDeviceSO : ScriptableObject
{
    public List<InputDevice> devices;
}
