using UnityEngine;

public enum Monsters
{
    Ghost,
    Poltergeist,
    Zombie
}

[System.Serializable]
public class Enemies
{
    public Monsters myType;
    public int number;
}