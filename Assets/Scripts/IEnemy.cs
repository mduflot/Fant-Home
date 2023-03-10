public interface IEnemy
{
    protected int health { get; }
    protected string name { get; }
    protected int damage { get; }
    protected int speed { get; }
    public void TakeDamage(int damageveil);
    public void TakeVeil(int damage);
}
