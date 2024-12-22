using UnityEngine;

[RequireComponent(typeof(IHealth))]
public class HealthRegenerator : MonoBehaviour, IDamageable
{
    [Tooltip("ダメージを受けてから再生を開始するまでの時間")]
    [SerializeField, Min(0)] float _regenDelay = 2;
    [Tooltip("毎秒の再生量")]
    [SerializeField, Min(0.0001f)] float _regenSpeed = 1;
    IHealth _health;

    private void OnEnable()
    {
        _health = GetComponent<IHealth>();
        InvokeRepeating(nameof(Regeneration), _regenDelay, 1 / _regenSpeed);
    }

    public void Damage(int damage)
    {
        CancelInvoke();
        InvokeRepeating(nameof(Regeneration), _regenDelay, 1 / _regenSpeed);
    }

    void Regeneration()
    {
        _health.Heal(1);
    }
}
