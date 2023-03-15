public enum Monsters
{
    GHOST,
    POLTERGEIST,
    ECTOPLASMA
}

[System.Serializable]
public class Enemies
{
    public Monsters myType;
    public int number;
}