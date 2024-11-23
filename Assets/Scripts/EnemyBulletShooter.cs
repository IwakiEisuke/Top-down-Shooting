using System.Collections;
using UnityEngine;

public class EnemyBulletShooter : BulletShooter
{
    [SerializeField] string _playerName = "Player";
    Transform _player;

    private void Start()
    {
        _player = GameObject.Find(_playerName).transform;
        StartCoroutine(Activate());
    }

    override protected void Update()
    {
        base.Update();
        TryShoot(_player.position);
    }

    IEnumerator Activate()
    {
        _isAttacking = true;
        yield return new WaitForSeconds(3);
        _isAttacking = false;
        yield return new WaitForSeconds(3);
        StartCoroutine(Activate());
    }
}
