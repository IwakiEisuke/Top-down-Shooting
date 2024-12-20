using UnityEngine;

/// <summary>
/// �ڐG���ɏ��ł���I�u�W�F�N�g�Ȃǒʏ�̕����Փ˂ł�
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
