using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Door : MonoBehaviour
{
    public GameObject DoorGO;
    public bool opened;
    
    public void ToggleDoor()
    {
        opened = !opened;
        
        if(DoorGO) DoorGO.SetActive(!opened);
        else transform.GetChild(0).gameObject.SetActive(!opened);
    }
}
