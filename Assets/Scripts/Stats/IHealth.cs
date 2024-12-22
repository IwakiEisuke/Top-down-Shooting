public interface IHealth
{
    void Damage(int damage);
    void Heal(int amount);
    void Death();
    int GetHealth();
    int GetMaxHealth();
}