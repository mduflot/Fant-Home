using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Door : MonoBehaviour
{
    public GameObject doorGO;
    public bool opened, locked;
    [HideInInspector] public List<Room> linkedRooms = new List<Room>();

    [SerializeField] private Material unlockedMat, lockedMat;

    //private MeshRenderer _meshRend;

    private void Awake()
    {
        //_meshRend = doorGO.GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        CheckIfLocked();
    }

    public void ToggleDoor(bool forceOpen = false)
    {
        if (locked) return;
        
        opened = forceOpen ? true : !opened;
        
        if(doorGO) doorGO.SetActive(!opened);
        else transform.GetChild(0).gameObject.SetActive(!opened);
        if (opened)
        {
            AudioManager.Instance.PlaySFXRandom("Door_Open", 0.8f, 1.2f);
        } else
        {

            AudioManager.Instance.PlaySFXRandom("Door_Close", 0.8f, 1.2f);
        }
    }

    public void CheckIfLocked()
    {
        foreach (var room in linkedRooms)
        {
            if (room.IsLocked)
            {
                locked = true;
                return;
            }
        }
        locked = false;
    }
}
