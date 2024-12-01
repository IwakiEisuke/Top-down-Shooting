using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damageAmount = 1;

    void DealDamage(GameObject target)
    {
        foreach (var d in target.GetComponents<IDamageable>())
        {
            d.TakeDamage(damageAmount);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        DealDamage(collision.gameObject);
    }
}
