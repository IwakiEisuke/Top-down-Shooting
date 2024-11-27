using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WarpHole : MonoBehaviour
{
    [SerializeField] WarpHole _target;
    List<Collider> _onWarpedObjects = new();

    private void OnTriggerEnter(Collider other)
    {
        if (!_onWarpedObjects.Contains(other))
        {
            _target.Warp(other);
        }
    }

    public void Warp(Collider other)
    {
        _onWarpedObjects.Add(other);

        if (other.TryGetComponent<NavMeshAgent>(out var agent))
        {
            agent.Warp(transform.position);
        }
        else
        {
            other.transform.position = transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _onWarpedObjects.Remove(other);
    }
}
