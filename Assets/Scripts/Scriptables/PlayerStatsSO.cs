using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptables/PlayerStats", order = 1)]
public class PlayerStatsSO : ScriptableObject
{
    public int maxHealth;
    public float invincibleTime;
}
