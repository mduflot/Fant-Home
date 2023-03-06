using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetails : MonoBehaviour
{
    public int playerID;
    public Vector3 spawnPos;
    public Color playerColor;

    private void Start()
    {
        transform.position = spawnPos;
        //GetComponent<Material>().color = playerColor;
    }
}
