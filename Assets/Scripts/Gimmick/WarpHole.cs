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
            // ワープ先で地面にめり込まないよう、ワープホールからの高さを取る
            var offset = other.transform.position - transform.position;
            offset.Scale(Vector3.up);
            _target.Warp(other, offset);
        }
    }

    public void Warp(Collider other, Vector3 offset)
    {
        _onWarpedObjects.Add(other);
        

        if (other.TryGetComponent<NavMeshAgent>(out var agent))
        {
            agent.Warp(transform.position + offset);
        }
        else
        {
            other.transform.position = transform.position + offset;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _onWarpedObjects.Remove(other);
    }
}
