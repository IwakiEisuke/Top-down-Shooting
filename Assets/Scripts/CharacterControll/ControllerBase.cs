using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ControllerBase : MonoBehaviour
{
    protected Rigidbody _rb;
    protected Vector3 _externalVelocity;
    protected Vector3 _angularVelocity;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        VelocityDecay();
        _rb.linearVelocity = Move();
        _rb.angularVelocity += Rotate();
    }

    protected virtual void VelocityDecay()
    {
        _externalVelocity *= 1 - _rb.linearDamping * Time.fixedDeltaTime;
        _angularVelocity *= 1 - _rb.angularDamping * Time.fixedDeltaTime;
    }

    protected virtual Vector3 Move()
    {
        return _externalVelocity;
    }

    protected virtual Vector3 Rotate()
    {
        return _angularVelocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!enabled) return;
        if (collision.rigidbody && collision.gameObject.GetComponent<ImpactSender>())
        {
            // ê¸ë¨ìxåvéZ
            _externalVelocity += collision.relativeVelocity / _rb.mass;

            // äpë¨ìxåvéZ
            var contact = collision.contacts[0];
            var torque = Vector3.Cross(contact.point - transform.position, collision.relativeVelocity);
            _rb.ResetInertiaTensor();
            var I = _rb.inertiaTensor;

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
}
