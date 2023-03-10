using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryDetector : MonoBehaviour
{
    public Action<Player> onTriggerEnterAction;
    public Action<Player> onTriggerExitAction;

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player")) return;

        Player player = other.GetComponent<Player>();
        onTriggerEnterAction?.Invoke(player);
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("Player")) return;

        Player player = other.GetComponent<Player>();
        onTriggerExitAction?.Invoke(player);
    }
}
