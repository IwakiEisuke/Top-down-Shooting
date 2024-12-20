using UnityEngine;

/// <summary>
/// 接触時に消滅するオブジェクトなど通常の物理衝突では
/// </summary>
public class ImpactReceiver : MonoBehaviour
{
    Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!enabled) return;
        if (collision.rigidbody && collision.gameObject.GetComponent<ImpactSender>())
        {
            _rb.AddForceAtPosition(collision.rigidbody.linearVelocity * collision.rigidbody.mass / _rb.mass, collision.GetContact(0).point, ForceMode.Impulse);
        }
    }
}
