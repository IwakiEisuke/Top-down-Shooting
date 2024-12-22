using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBulletShooter : BulletShooter
{
    [SerializeField] GunPointer _pointer;

    protected new void Update()
    {
        base.Update();
        TryShoot(_pointer.HitPosition);
    }

    private void OnFire(InputValue value)
    {
        _isAttacking = value.isPressed;
    }
}
