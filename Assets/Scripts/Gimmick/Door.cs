using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider), typeof(NavMeshObstacle))]
public class Door : MonoBehaviour
{
    [SerializeField] float _moveY;
    [SerializeField] float _duration;
    [SerializeField] Collider _blocker;
    Vector3 _targetPos;

    private void Start()
    {
        _targetPos = transform.position;
    }

    public void Open()
    {
        if (_blocker) _blocker.enabled = true;
        _targetPos += Vector3.up * _moveY;
        transform.DOMoveY(_targetPos.y, _duration)
            .OnComplete(() => { if (_blocker) _blocker.enabled = false; });
    }

    public void Close()
    {
        if (_blocker) _blocker.enabled = false;
        _targetPos += Vector3.down * _moveY;
        transform.DOMoveY(_targetPos.y, _duration);
    }

    private void OnValidate()
    {
        if (_blocker)
        {
            _blocker.SetBoundsOnMesh(gameObject, gameObject.transform.up * _moveY);
        }
        if (TryGetComponent<NavMeshObstacle>(out var navMeshObstacle))
        {
            navMeshObstacle.SetBoundsOnMesh(gameObject);
        }
    }
}
