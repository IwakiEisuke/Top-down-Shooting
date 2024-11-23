using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    [SerializeField] GameObject _bulletPref;
    [SerializeField] PointerC _pointer;
    [SerializeField] float _shootForce = 10;
    [SerializeField] float _interval = 0.2f;
    bool _isAttacking = false;
    float _timeSinceShoot = 0;

    private void OnFire(InputValue value)
    {
        _isAttacking = value.isPressed;
    }

    private void Update()
    {
        if (_isAttacking && _interval < _timeSinceShoot)
        {
            _timeSinceShoot = 0;

            var bullet = Instantiate(_bulletPref);
            bullet.transform.position = transform.position;
            bullet.transform.forward = _pointer.Position - transform.position;
            var rb = bullet.GetComponent<Rigidbody>();
            rb.linearVelocity = bullet.transform.forward * _shootForce;
        }

        _timeSinceShoot += Time.deltaTime;
    }
}
