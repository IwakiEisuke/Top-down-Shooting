using UnityEngine;

public class Bouncy : MonoBehaviour
{
    [SerializeField] Vector3 _bounceVector;
    [SerializeField] float _bounceForce;
    private void OnCollisionEnter(Collision collision)
    {
        collision.rigidbody.AddForce(_bounceVector * _bounceForce);
    }
}
