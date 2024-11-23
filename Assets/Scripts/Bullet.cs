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
        // Šù‚É‰½‚©‚µ‚ç‚Éƒ_ƒ[ƒW‚ğ—^‚¦‚Ä‚¢‚½‚çUŒ‚‚Å‚«‚È‚¢
        if (!_isAttacked && collision.gameObject.TryGetComponent<StatsManager>(out var stats))
        {
            if (_rb.linearVelocity.sqrMagnitude > 10) // ˆê’è‘¬“x‚ğ‚Á‚Ä‚¢‚È‚¯‚ê‚ÎUŒ‚‚Å‚«‚È‚¢
            {
                stats.DealDamage(_damage);
                _isAttacked = true;
                Destroy(gameObject);
            }
        }
    }
}
