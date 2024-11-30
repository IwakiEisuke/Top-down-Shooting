using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _speed = 10;
    [SerializeField] float _jumpForce = 10;
    [SerializeField] int _jumpCount = 2;
    [SerializeField] LayerMask _canGroundedLayer;

    Vector3 _input;
    Rigidbody _rb;
    Renderer _renderer;
    int _currJumpCount;

    Vector3 _respawnPoint;
    Vector3 _externalVelocity;
    Vector3 _angularVelocity;
    Vector3 _jumpVelocity;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _respawnPoint = transform.position;
        _renderer = GetComponent<Renderer>();
    }

    private void FixedUpdate()
    {
        _externalVelocity = Vector3.Lerp(_externalVelocity, Vector3.zero, 5 * Time.fixedDeltaTime);
        _angularVelocity = Vector3.Lerp(_angularVelocity, Vector3.zero, 5 * Time.fixedDeltaTime);
        _jumpVelocity += Physics.gravity * Time.fixedDeltaTime;

        var inputVelocity = _input * _speed;

        _rb.linearVelocity = inputVelocity + _externalVelocity + _jumpVelocity;
        Debug.Log(_rb.linearVelocity);
        //_rb.angularVelocity += _angularVelocity;
    }

    private void OnCollisionStay(Collision collision)
    {
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody)
        {
            // ü‘¬“xŒvŽZ
            _externalVelocity += -collision.impulse / _rb.mass;

            // Šp‘¬“xŒvŽZ
            var contact = collision.contacts[0];
            var torque = Vector3.Cross(contact.point - transform.position, -collision.impulse);
            _rb.ResetInertiaTensor();
            var I = _rb.inertiaTensor;

            torque = Quaternion.FromToRotation(collision.impulse, Vector3.up).eulerAngles;

            _angularVelocity += Vector3.Scale(torque, Reciprocal(I));
        }
    }

    private Vector3 Reciprocal(Vector3 v)
    {
        return new Vector3(
            v.x != 0 ? 1f / v.x : 0, 
            v.y != 0 ? 1f / v.y : 0, 
            v.z != 0 ? 1f / v.z : 0);
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
