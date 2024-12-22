using System.Collections;
using UnityEngine;

public class EnemyBulletShooter : BulletShooter
{
    [SerializeField] string _playerName = "Player";
    [Tooltip("一回の射撃で何秒間撃つか")]
    [SerializeField] float _shootingDuration = 1f;
    [Tooltip("射撃行動をとれるようになるまでのインターバル")]
    [SerializeField] float _firingInterval = 3f;
    Transform _player;
    Coroutine _currentCoroutine;

    private void Start()
    {
        _player = GameObject.Find(_playerName).transform;
    }

    override protected void Update()
    {
        base.Update();
        TryShoot(_player.position);
    }

    IEnumerator Activate()
    {
        _isAttacking = true;
        yield return new WaitForSeconds(_shootingDuration);
        _isAttacking = false;
        yield return new WaitForSeconds(_firingInterval);
        _currentCoroutine = null;
    }

    public void Shoot()
    {
        _currentCoroutine ??= StartCoroutine(Activate());
    }
}
