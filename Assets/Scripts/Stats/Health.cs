using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 体力を管理するコンポーネント
/// </summary>
public class Health : MonoBehaviour, IDamageable, IHealth
{
    [SerializeField] int _maxHealth = 5;
    public UnityEvent onTakeDamage;
    public UnityEvent onHeal;
    public UnityEvent onDie;
    int _health = 5;

    private void Awake()
    {
        _health = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        onTakeDamage.Invoke();

        if (_health <= 0) Die();
    }

    public void Heal(int amount)
    {
        _health += amount;
        onHeal.Invoke();
        
        if (_health > _maxHealth) _health = _maxHealth;
    }

    private void Die()
    {
        onDie.Invoke();
    }

    public int GetHealth() => _health;
    public int GetMaxHealth() => _maxHealth;
}
