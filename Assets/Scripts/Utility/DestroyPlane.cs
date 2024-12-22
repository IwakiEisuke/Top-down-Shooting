using UnityEngine;

public class DestroyPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IHealth>(out var health))
        {
            health.Death();
        }
        else
        {
            Destroy(other.gameObject);
        }
    }
}
