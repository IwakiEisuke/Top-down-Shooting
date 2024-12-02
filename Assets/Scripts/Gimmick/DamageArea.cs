using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [SerializeField] int _damage;
    [SerializeField] float _timeToDamage;
    Dictionary<StatsManager, float> _timer = new();

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<StatsManager>(out var stats))
        {
            _timer.TryAdd(stats, 0);
            _timer[stats] += Time.deltaTime;

            if (_timer[stats] > _timeToDamage)
            {
                _timer[stats] = 0;
                stats.DealDamage(_damage);
                Debug.Log($"deal damage to {stats.name}");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<StatsManager>(out var stats))
        {
            //if (_timer.ContainsKey(stats))
            {
                _timer.Remove(stats);
            }
        }
    }
}
