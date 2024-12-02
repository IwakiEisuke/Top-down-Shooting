using UnityEngine;

/// <summary>
/// ƒ_ƒ[ƒW‚ğó‚¯æ‚é
/// </summary>
public class DamageReceiver : MonoBehaviour, IDamageable
{
    IHealth _health;

    private void Awake()
    {
        _health = GetComponentInParent<Health>();
    }

    public void Damage(int amount)
    {
        _health.Damage(amount);
    }
}
