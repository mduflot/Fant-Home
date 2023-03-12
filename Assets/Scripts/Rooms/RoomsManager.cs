using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsManager : MonoBehaviour
{
    [SerializeField] private WaveTool waveTool;
    [SerializeField] private WaveBeginInfos[] wavesUnlock;
    private Room[] rooms;

    private void Awake()
    {
        rooms = FindObjectsOfType<Room>();
        Debug.Log(rooms.Length + " room(s) found.");

        if (!waveTool) waveTool = FindObjectOfType<WaveTool>();
        waveTool.NewWave += TriggerWave;
    }

    private void TriggerWave(int index)
    {
        foreach (var wave in wavesUnlock)
        {
            if (wave.wave == index)
            {
                foreach (var room in wave.roomsToOpen)
                {
                    // ReSharper disable once Unity.NoNullPropagation
                    room?.UnlockRoom();
                }
            }
        }
    }
}

[Serializable]
public class WaveBeginInfos
{
    public int wave;
    public Room[] roomsToOpen;
}
