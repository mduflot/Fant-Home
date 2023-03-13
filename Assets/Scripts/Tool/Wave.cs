using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public int waveNumber;
    public float spawnTime;
    public Enemies[] enemies = new Enemies[3];
}
