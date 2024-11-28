using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TransparentCeil : MonoBehaviour
{
    [SerializeField] float _roomHeight;
    [SerializeField] BoxCollider _roomCollider;
    [SerializeField] float _startAlpha;
    [SerializeField] float _endAlpha;
    [SerializeField] float _transitionTime;
    Material _material;
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

    private void ChangeAlpha(float alpha)
    {
        _material = _ceilMesh.material;
        var newColor = _material.color;
        newColor.a = alpha;
        _material.DOColor(newColor, _transitionTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        ChangeAlpha(_endAlpha);
    }

    private void OnTriggerExit(Collider other)
    {
        ChangeAlpha(_startAlpha);
    }

    private void OnValidate()
    {
        Set();
    }
}
