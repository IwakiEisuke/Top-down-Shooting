using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBulletShooter : BulletShooter
{
    [SerializeField] GunPointer _pointer;

    override protected void Update()
    {
        base.Update();
        TryShoot(_pointer.HitPosition);
    }

    private void OnFire(InputValue value)
    {
        _isAttacking = value.isPressed;
    }
}
