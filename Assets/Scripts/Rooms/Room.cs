using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool IsLocked => locked;
    public int GetIndex => index;
    public List<GameObject> Floors;
    
    [SerializeField] private bool locked;
    [SerializeField] private int index;
    [SerializeField] private FogOfWarTrigger[] fog;
    [SerializeField] private Door[] doors;
    [SerializeField] private EntryDetector[] entries;

    private List<Player> curPlayersInRoom = new();

    private void Awake()
    {
        foreach (var door in doors)
        {
            door.linkedRooms.Add(this);
            door.locked = locked;
        }

        foreach (var entry in entries)
        {
            entry.onTriggerEnterAction += PlayerEnter;
        }
    }

    [ContextMenu("UnlockDoor")]
    public void UnlockRoom()
    {
        locked = false;
        foreach (var door in doors)
        {
            door.locked = false;
        }
    }

    public void PlayerEnter(Player player)
    {
        if (player.curRoom) player.curRoom.PlayerExit(player);

        player.curRoom = this;

        if (!curPlayersInRoom.Contains(player)) curPlayersInRoom.Add(player);

        EnableFog(true);
    }

    private void PlayerExit(Player player)
    {
        curPlayersInRoom.Remove(player);
        CheckFogState();
    }

    private void CheckFogState()
    {
        EnableFog(curPlayersInRoom.Count > 0);
    }

    private void EnableFog(bool enable)
    {
        foreach (var f in fog)
        {
            if (enable) f.DisplayRoom();
            else f.HideRoom();
        }
    }
}