using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Renderer _model;
    [SerializeField] LayerMask _canGroundedLayer;

    [Header("Jump")]
    [SerializeField] float _jumpForce = 10;
    [SerializeField] int _jumpCount = 2;
    [SerializeField] float _gravityScale = 1;

    [Header("Move")]
    [SerializeField] float _speed = 10;
    [SerializeField] float _maxAccel = 150;
    [SerializeField] float _maxVerticalAccel = 150;
    [SerializeField] float _maxFallVelocity = 30;

    Rigidbody _rb;
    Vector3 _input;
    Vector3 _respawnPoint;
    int _currJumpCount;
    bool _isGrounded;

    private void Start()
    {
        _respawnPoint = transform.position;
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        var goalVel = _input * _speed;
        var currHVel = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
        var currVVel = Vector3.up * _rb.linearVelocity.y;

        var newHVel = Vector3.MoveTowards(currHVel, goalVel, _maxAccel * Time.fixedDeltaTime);
        var newVVel = Vector3.MoveTowards(currVVel, Physics.gravity * _maxFallVelocity, _maxVerticalAccel * Time.fixedDeltaTime);
        if (_isGrounded) newVVel = Physics.gravity * Time.fixedDeltaTime;

        _rb.linearVelocity = newHVel + newVVel;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (IsGrounded())
        {
            _currJumpCount = 0;
            _isGrounded = true;
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (IsGrounded())
    //    {
    //        _currJumpCount = 0;
    //        _isGrounded = true;
    //    }
    //}

    private void OnCollisionExit(Collision collision)
    {
        _isGrounded = false;
    }

    private bool IsGrounded()
    {
        // ê⁄ínîªíË
        var stepHeight = 0.1f;
        var halfExtent = 0.5f;
        var extentMargin = 0.05f;
        var extents = new Vector3(halfExtent - extentMargin, halfExtent, halfExtent - extentMargin);

        if (_rb.linearVelocity.y <= 0)
        {
            if (Physics.BoxCast(_model.bounds.center + Vector3.up * stepHeight, extents, Vector3.down, Quaternion.identity, halfExtent + stepHeight, _canGroundedLayer))
            {
                return true;
            }
        }

        return false;
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
            _rb.linearVelocity += Vector3.up * (_jumpForce - _rb.linearVelocity.y);
            _currJumpCount++;
            _isGrounded = false;
        }
    }

    private void OnRestart(InputValue value)
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.MovePosition(_respawnPoint);
    }
}
