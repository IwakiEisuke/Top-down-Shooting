using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controllerに衝撃を与える能力を持たせるコンポーネント
/// </summary>
public class KnockbackSender : MonoBehaviour
{
    [SerializeField, Range(0, 100)] float _knockbackTime = 0.5f;
    Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Knockback>(out var k))
        {
            k.ApplyKnockback(_knockbackTime, _rb.linearVelocity * _rb.mass, collision.rigidbody);
        }
    }
}
