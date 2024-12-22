﻿using UnityEngine;

/// <summary>
/// ダメージを受け取る
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
