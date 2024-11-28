using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TransparentCeil : MonoBehaviour
{
    [SerializeField] float _roomHeight;
    [SerializeField] BoxCollider _roomCollider;
    Renderer _ceilMesh;

    private void Reset()
    {
        Set();
    }

    [ContextMenu(nameof(Set))]
    private void Set()
    {
        _ceilMesh = GetComponent<Renderer>();
        _roomCollider = GetComponent<BoxCollider>();

        var center = _ceilMesh.bounds.center - _ceilMesh.transform.position;
        center.y -= (_ceilMesh.bounds.size.y + _roomHeight) / 2;
        var size = _ceilMesh.bounds.size;
        size.y = _roomHeight;

        _roomCollider.center = center;
        _roomCollider.size = size;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnValidate()
    {
        Set();
    }
}
