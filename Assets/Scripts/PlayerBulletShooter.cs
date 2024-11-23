using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBulletShooter : BulletShooter
{
    [SerializeField] PointerC _pointer;

    override protected void Update()
    {
        base.Update();
        TryShoot(_pointer.Position);
    }

    private void OnFire(InputValue value)
    {
        _isAttacking = value.isPressed;
    }
}
