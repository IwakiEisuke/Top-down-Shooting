using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] GameObject _bulletPref;
    [SerializeField, FormerlySerializedAs("_shootForce")] float _bulletSpeed = 10;
    [Tooltip("射撃行動時に弾を撃つ間隔")]
    [SerializeField] float _bulletInterval = 0.2f;
    [Tooltip("弾のバラつき度合い")]
    [SerializeField] float _angleRandomness;
    [SerializeField] bool _doRandomizeXAxisRotation;
    protected bool _isAttacking = false;
    float _timeSinceShoot = 0;

    protected void TryShoot(Vector3 targetPos)
    {
        if (_isAttacking && _bulletInterval < _timeSinceShoot)
        {
            _timeSinceShoot = 0;

            var bullet = Instantiate(_bulletPref);
            bullet.transform.position = transform.position;
            bullet.transform.forward = targetPos - transform.position;
            var rb = bullet.GetComponent<Rigidbody>();
            // 弾をバラつかせる
            var xr = _doRandomizeXAxisRotation ? Random.Range(-_angleRandomness / 2, _angleRandomness / 2) : 0;
            var yr = Random.Range(-_angleRandomness / 2, _angleRandomness / 2);
            var forward = Quaternion.Euler(xr, yr, 0) * bullet.transform.forward;
            rb.linearVelocity = forward * _bulletSpeed;
        }
    }

    protected void Update()
    {
        _timeSinceShoot += Time.deltaTime;
    }
}
