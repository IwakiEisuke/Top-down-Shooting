using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [SerializeField] int _damage;
    [SerializeField] float _timeToDamage;
    Dictionary<IHealth, float> _timer = new();

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<IHealth>(out var stats))
        {
            _timer.TryAdd(stats, 0);
            _timer[stats] += Time.deltaTime;

            if (_timer[stats] > _timeToDamage)
            {
                _timer[stats] = 0;
                stats.Damage(_damage);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IHealth>(out var stats))
        {
            {
                _timer.Remove(stats);
            }
        }
    }
}
