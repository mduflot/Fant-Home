public enum Monsters
{
    GHOST,
    POLTERGEIST,
    ZOMBIE
}

[System.Serializable]
public class Enemies
{
    public Monsters myType;
    public int number;
}