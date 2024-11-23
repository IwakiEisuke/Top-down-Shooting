using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int _damage = 1;
    [SerializeField] float _existDuration = 10f;
    [SerializeField] bool _destroyOnCollide;
    [SerializeField] LayerMask _reflects;
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

        // layerMaskは何桁目に1があるか、gameObject.layerはレイヤー番号そのままの整数
        if ((_reflects &= 1 << collision.gameObject.layer) != 0)
        {
            // 速度に反射ベクトルを代入
            _rb.linearVelocity = Vector3.Reflect(-collision.relativeVelocity, collision.GetContact(0).normal);
            transform.forward = _rb.linearVelocity; // 進行方向に向かせる
        }

        var layerMask = ~_reflects; // なぜかインラインだと反転できない…
        if (_destroyOnCollide && (layerMask &= 1 << collision.gameObject.layer) != 0)
        {
            Destroy(gameObject);
        }
    }
}
