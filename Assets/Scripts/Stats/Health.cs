using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

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

    [SerializeField] float _damageTime = 0.1f;
    Color _originalColor;
    Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _originalColor = _renderer.material.color;

        _health = _maxHealth;
    }

    public void Damage(int damage)
    {
        if (!enabled) return;

        _health -= damage;
        onDamage.Invoke();
        DamageHitEffect();

        if (_health <= 0) Death();
    }

    void DamageHitEffect()
    {
        _renderer.material.color = Color.red;
        _renderer.material.DOColor(_originalColor, _damageTime);
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
