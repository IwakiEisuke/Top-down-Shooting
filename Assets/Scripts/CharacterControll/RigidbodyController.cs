using UnityEngine;

[DefaultExecutionOrder(100)]
public class RigidbodyController : MonoBehaviour
{
    Rigidbody _rb;
    Vector3 _linearVelocity;
    Vector3 _angularVelocity;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
        ResetVelocity();
    }

    void ResetVelocity()
    {
        _linearVelocity = Vector3.zero;
        _angularVelocity = Vector3.zero;
    }

    void Move()
    {
        _rb.linearVelocity = _linearVelocity;
        _rb.angularVelocity = _angularVelocity;
    }

    public void AddVelocity(Vector3 linear, Vector3 angular)
    {
        _linearVelocity += linear;
        _angularVelocity += angular;
    }
}
