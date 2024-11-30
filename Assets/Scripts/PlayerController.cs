using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : ControllerBase
{
    [SerializeField] float _speed = 10;
    [SerializeField] float _jumpForce = 10;
    [SerializeField] int _jumpCount = 2;
    [SerializeField] LayerMask _canGroundedLayer;
    Vector3 _input;
    Vector3 _respawnPoint;
    Vector3 _jumpVelocity;
    Renderer _renderer;
    int _currJumpCount;

    private void Start()
    {
        _respawnPoint = transform.position;
        _rb = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
    }

    protected override void VelocityDecay()
    {
        base.VelocityDecay();
        _jumpVelocity += Physics.gravity * Time.fixedDeltaTime;
    }

    protected override Vector3 Move()
    {
        _jumpVelocity += Physics.gravity * Time.fixedDeltaTime;
        var inputVelocity = _input * _speed;
        return inputVelocity + _externalVelocity + _jumpVelocity;
    }

    protected override Vector3 Rotate()
    {
        return _angularVelocity;
    }

    private void OnCollisionStay(Collision collision)
    {
        // ê⁄ínîªíË
        var stepHeight = 0.1f;
        var halfExtent = 0.5f;
        var extentMargin = 0.05f;
        var extents = new Vector3(halfExtent - extentMargin, halfExtent, halfExtent - extentMargin);

        if (_rb.linearVelocity.y <= 0)
        {
            if (Physics.BoxCast(_renderer.bounds.center + Vector3.up * stepHeight, extents, Vector3.down, Quaternion.identity, halfExtent + stepHeight, _canGroundedLayer))
            {
                _currJumpCount = 0;
                _jumpVelocity = Vector3.zero;
            }
        }
    }

    /*
     *     inputs
     */

    private void OnMove(InputValue value)
    {
        var axis = value.Get<Vector2>();
        _input = new Vector3(axis.x, 0, axis.y);
    }

    private void OnJump(InputValue value)
    {
        if (_currJumpCount < _jumpCount)
        {
            _jumpVelocity = Vector3.up * _jumpForce;
            _currJumpCount++;
        }
    }

    private void OnRestart(InputValue value)
    {
        transform.position = _respawnPoint;
    }
}
