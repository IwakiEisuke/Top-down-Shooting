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
        // 既に何かしらにダメージを与えていたら攻撃できない
        if (!_isAttacked && collision.gameObject.TryGetComponent<StatsManager>(out var stats))
        {
            if (_rb.linearVelocity.sqrMagnitude > 10) // 一定速度を持っていなければ攻撃できない
            {
                stats.DealDamage(_damage);
                _isAttacked = true;
                Destroy(gameObject);
            }
        }
    }
}
