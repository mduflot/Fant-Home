using Unity.Entities;
using UnityEngine;

public enum MonsterType
{
    Ghost,
    Poltergeist,
    Zombie
}

[CreateAssetMenu(fileName = "MonsterStats", menuName = "Scriptables/MonsterStats", order = 2)]
public class GhostSO: ScriptableObject
{
    public int health;
    public int damage;
    public float speed;
    public MonsterType key;
}