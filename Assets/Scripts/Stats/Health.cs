using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 体力を管理するコンポーネント
/// </summary>
public class Health : MonoBehaviour, IHealth
{
    [SerializeField] int _maxHealth = 5;

    [Space(16)]
    public UnityEvent onDamage;
    public UnityEvent onHeal;
    public UnityEvent onDie;
    int _health = 5;

    private void Awake()
    {
        _health = _maxHealth;
    }

    public void Damage(int damage)
    {
        if (!enabled) return;

        _health -= damage;
        onDamage.Invoke();

        if (_health <= 0) Death();
    }

    public void Heal(int amount)
    {
        _health += amount;
        onHeal.Invoke();
        
        if (_health > _maxHealth) _health = _maxHealth;
    }

    public void Death()
    {
        onDie.Invoke();
    }

    public int GetHealth() => _health;
    public int GetMaxHealth() => _maxHealth;
}
