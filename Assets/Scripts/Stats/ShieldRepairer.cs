using UnityEngine;

public class ShieldRepairer : MonoBehaviour, IDamageable
{
    [SerializeField] float _rebuildDelay = 5f;
    [SerializeField] Health _repairTarget;

    void OnEnable()
    {
        Invoke(nameof(Repair), _rebuildDelay);
    }

    public void Repair()
    {
        _repairTarget.Heal(_repairTarget.GetMaxHealth() / 2);
        _repairTarget.gameObject.SetActive(true);
        _repairTarget.enabled = true;
        enabled = false;
    }

    public void Damage(int damage)
    {
        CancelInvoke();
        Invoke(nameof(Repair), _rebuildDelay);
    }
}
