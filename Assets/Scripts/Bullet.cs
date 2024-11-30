using UnityEngine;
using UnityEngine.AI;

public class Bullet : MonoBehaviour
{
    [SerializeField] int _damage = 1;
    [SerializeField] float _existDuration = 10f;
    [SerializeField] bool _destroyOnCollide;
    [SerializeField] LayerMask _reflects;
    [SerializeField] TrailRenderer _trail;
    Rigidbody _rb;
    bool _isAttacked;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Invoke(nameof(DestroyBullet), _existDuration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody)
        {
            Debug.Log($"b : {collision.rigidbody.mass} {collision.impulse} {collision.relativeVelocity}");
            collision.rigidbody.AddForce(-collision.impulse, ForceMode.Impulse);
        }

        if (collision.gameObject.TryGetComponent<NavMeshAgent>(out var agent) && collision.gameObject.TryGetComponent<AgentSettings>(out var settings))
        {
            agent.velocity -= collision.impulse / collision.rigidbody.mass / settings.LinearDrag;
        }

        // ���ɉ�������Ƀ_���[�W��^���Ă�����U���ł��Ȃ�
        if (!_isAttacked && collision.gameObject.TryGetComponent<StatsManager>(out var stats))
        {
            if (_rb.linearVelocity.sqrMagnitude > 10) // ��葬�x�������Ă��Ȃ���΍U���ł��Ȃ�
            {
                stats.DealDamage(_damage);
                _isAttacked = true;
                DestroyBullet();
            }
        }

        // ���e����
        // layerMask�͉����ڂ�1�����邩�AgameObject.layer�̓��C���[�ԍ����̂܂܂̐���
        if ((_reflects &= 1 << collision.gameObject.layer) != 0)
        {
            // ���x�ɔ��˃x�N�g������
            _rb.linearVelocity = Vector3.Reflect(-collision.relativeVelocity, collision.GetContact(0).normal);
            transform.forward = _rb.linearVelocity; // �i�s�����Ɍ�������
        }

        var layerMask = ~_reflects; // �Ȃ����C�����C�����Ɣ��]�ł��Ȃ��c
        if (_destroyOnCollide && (layerMask &= 1 << collision.gameObject.layer) != 0)
        {
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        if (_trail)
        {
            _trail.transform.parent = null;
            _trail.autodestruct = true;
        }
        Destroy(gameObject);
    }
}
