using System;
using Unity.Collections;
using UnityEngine;

public class RoomsManager : MonoBehaviour
{
    public static RoomsManager Instance;

    [SerializeField] private WaveTool waveTool;
    [SerializeField] private WaveBeginInfos[] wavesUnlock;
    [ReadOnly] public Room[] rooms;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);

        Instance = this;

        rooms = FindObjectsOfType<Room>();
        Debug.Log(rooms.Length + " room(s) found. :)");

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