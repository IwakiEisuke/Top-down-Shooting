using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _speed = 10;
    [SerializeField] float _jumpForce = 10;
    [SerializeField] int _jumpCount = 2;

    Vector3 _input;
    Rigidbody _rb;
    int _currJumpCount;

    Vector3 _respawnPoint;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _respawnPoint = transform.position;
    }

    private void Update()
    {
        _rb.linearVelocity = _input * _speed + new Vector3(0, _rb.linearVelocity.y, 0);
    }

    private void OnCollisionStay(Collision collision)
    {
        var stepHeight = 0.1f;
        var halfExtent = 0.5f;
        var extentMargin = 0.05f;
        var extents = new Vector3(halfExtent - extentMargin, halfExtent, halfExtent - extentMargin);

        if (_rb.linearVelocity.y <= 0)
        {
            if (Physics.BoxCast(transform.position + Vector3.up * stepHeight, extents, Vector3.down, Quaternion.identity, halfExtent + stepHeight))
            {
                _currJumpCount = 0;
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
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, _jumpForce, _rb.linearVelocity.z);
            _currJumpCount++;
        }
    }

    private void OnRestart(InputValue value)
    {
        transform.position = _respawnPoint;
    }
}
