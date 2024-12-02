public interface IHealth : IDamageable
{
    void Heal(int amount);
    void Death();
    int GetHealth();
    int GetMaxHealth();
}