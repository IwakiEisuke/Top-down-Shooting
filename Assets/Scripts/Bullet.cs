using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int _damage = 1;
    [SerializeField] float _existDuration = 10f;
    Rigidbody _rb;
    bool _isAttacked;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Destroy(gameObject, _existDuration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ���ɉ�������Ƀ_���[�W��^���Ă�����U���ł��Ȃ�
        if (!_isAttacked && collision.gameObject.TryGetComponent<StatsManager>(out var stats))
        {
            if (_rb.linearVelocity.sqrMagnitude > 10) // ��葬�x�������Ă��Ȃ���΍U���ł��Ȃ�
            {
                stats.DealDamage(_damage);
                _isAttacked = true;
                Destroy(gameObject);
            }
        }
    }
}
