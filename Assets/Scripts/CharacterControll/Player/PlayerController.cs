using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Renderer _model;
    [SerializeField] RigidbodyController _controller;
    [SerializeField] float _speed = 10;
    [SerializeField] float _jumpForce = 10;
    [SerializeField] int _jumpCount = 2;
    [SerializeField] LayerMask _canGroundedLayer;
    Rigidbody _rb;
    Vector3 _input;
    Vector3 _respawnPoint;
    Vector3 _jumpVelocity;
    int _currJumpCount;

    private void Start()
    {
        _respawnPoint = transform.position;
        _rb = _controller.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        _jumpVelocity += Physics.gravity * Time.fixedDeltaTime;
        _controller.AddVelocity(_input * _speed + _jumpVelocity, Vector3.zero);
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
            if (Physics.BoxCast(_model.bounds.center + Vector3.up * stepHeight, extents, Vector3.down, Quaternion.identity, halfExtent + stepHeight, _canGroundedLayer))
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
