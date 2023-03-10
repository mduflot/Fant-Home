using UnityEngine;

public enum MonsterType
{
    Ghost,
    Poltergeist,
    Zombie
}

[CreateAssetMenu(fileName = "new GhostStatsSO", menuName = "Scriptables/GhostsSO/GhostsStatsSO", order = 2)]
public class GhostSO: ScriptableObject
{
    public int health;
    public int damage;
    public float speed;
    public MonsterType key;
}