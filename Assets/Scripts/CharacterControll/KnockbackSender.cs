using System;
using UnityEngine;

/// <summary>
/// Controllerに衝撃を与える能力を持たせるコンポーネント
/// </summary>
public class KnockbackSender : MonoBehaviour
{
    [SerializeField, Range(0, 100)] float _knockbackTime = 0.5f;
    Rigidbody _rb;
    Vector3 _prevVel;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _prevVel = _rb.linearVelocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Knockback>(out var k))
        {
            k.ApplyKnockback(_knockbackTime, _prevVel * _rb.mass);
        }
    }
}
