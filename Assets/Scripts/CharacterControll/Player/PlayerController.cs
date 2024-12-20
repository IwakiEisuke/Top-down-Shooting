using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask _canGroundedLayer;

    [Header("Locomotion")]
    [SerializeField] float _maxSpeed = 10;
    [SerializeField] float _maxAccel = 150;
    [SerializeField] float _inAirAccel = 50;
    [SerializeField] AnimationCurve _airDrag;

    [Header("Jumping")]
    [SerializeField] float _jumpSpeed = 10;
    [SerializeField] int _jumpCount = 2;
    [SerializeField] float _gravity;
    [SerializeField] float _maxFallSpeed = 30;
    [SerializeField] Knockback k;

    Rigidbody _rb;
    Vector3 _input;
    Vector3 _respawnPoint;
    int _currJumpCount;
    bool _isGrounded;
    bool _isKnockback;

    private void Start()
    {
        _respawnPoint = transform.position;
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        _rb.linearVelocity = Vector3.MoveTowards(_rb.linearVelocity, Vector3.zero, _airDrag.Evaluate(_rb.linearVelocity.magnitude) * Time.fixedDeltaTime);

        var currHVel = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
        var currVVel = Vector3.up * _rb.linearVelocity.y;

        var newHVel = Vector3.MoveTowards(currHVel, _input * _maxSpeed, (_isGrounded && !k.IsKnockback ? _maxAccel : _inAirAccel) * Time.fixedDeltaTime);
        var newVVel = Vector3.MoveTowards(currVVel, Physics.gravity.normalized * _maxFallSpeed, _gravity * Time.fixedDeltaTime);

        _rb.linearVelocity = newHVel + newVVel;
    }

    private bool IsGrounded(Collision collision)
    {
        if ((collision.gameObject.layer & _canGroundedLayer) != 0) return false;

        if (collision.GetContact(0).normal.y > 0.5f)
        {
            _currJumpCount = 0;
            _isGrounded = true;
            return true;
        }
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        IsGrounded(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        IsGrounded(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        _isGrounded = false;
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
            _rb.linearVelocity += Vector3.up * (_jumpSpeed - _rb.linearVelocity.y);
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
