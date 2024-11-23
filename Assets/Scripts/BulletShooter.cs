using UnityEngine;
using UnityEngine.InputSystem;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] GameObject _bulletPref;
    [SerializeField] float _shootForce = 10;
    [SerializeField] float _interval = 0.2f;
    protected bool _isAttacking = false;
    float _timeSinceShoot = 0;

    protected void TryShoot(Vector3 targetPos)
    {
        if (_isAttacking && _interval < _timeSinceShoot)
        {
            _timeSinceShoot = 0;

            var bullet = Instantiate(_bulletPref);
            bullet.transform.position = transform.position;
            bullet.transform.forward = targetPos - transform.position;
            var rb = bullet.GetComponent<Rigidbody>();
            rb.linearVelocity = bullet.transform.forward * _shootForce;
        }
    }

    virtual protected void Update()
    {
        _timeSinceShoot += Time.deltaTime;
    }
}
