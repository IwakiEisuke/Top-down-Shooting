using System.Collections;
using UnityEngine;

public class EnemyBulletShooter : BulletShooter
{
    [SerializeField] string _playerName = "Player";
    [Tooltip("���̎ˌ��ŉ��b�Ԍ���")]
    [SerializeField] float _shootingDuration = 1f;
    [Tooltip("�ˌ��s�����Ƃ��悤�ɂȂ�܂ł̃C���^�[�o��")]
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