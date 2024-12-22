using UnityEngine;

/// <summary>
/// IDamageable����������Ă���N���X�Ƀ_���[�W��^����
/// </summary>
public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damageAmount = 1;

    void DealDamage(GameObject target)
    {
        foreach (var damageable in target.GetComponents<IDamageable>())
        {
            var behaviour = damageable as MonoBehaviour;
            if (behaviour.enabled)
                damageable.Damage(damageAmount);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        DealDamage(collision.gameObject);
    }
}
