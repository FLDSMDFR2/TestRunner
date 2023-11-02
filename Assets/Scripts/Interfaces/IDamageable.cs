public interface IDamageable
{
    public abstract void SetHealth(int Health);
    public abstract int GetHealth();
    public abstract void TakeDamage(IDamager damage);
}
