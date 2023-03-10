public interface IEnemy
{
    protected float health { get; }
    protected string name { get; }
    protected float damage { get; }
    protected int speed { get; }
    public void TakeDamage(float damageveil);
    public void TakeVeil(float damage);
}
