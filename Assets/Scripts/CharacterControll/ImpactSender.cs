using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controllerに衝撃を与える能力を持たせるコンポーネント
/// </summary>
public class ImpactSender : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody)
        {
            collision.rigidbody.AddForce(-collision.impulse, ForceMode.Impulse);
        }

        if (collision.gameObject.TryGetComponent<NavMeshAgent>(out var agent) && collision.gameObject.TryGetComponent<AgentSettings>(out var settings))
        {
            agent.velocity -= collision.impulse / collision.rigidbody.mass / settings.LinearDrag;
        }
    }
}
