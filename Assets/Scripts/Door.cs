using DG.Tweening;
using UnityEngine;

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
        _blocker.enabled = true;
        _targetPos += Vector3.up * _moveY;
        transform.DOMoveY(_targetPos.y, _duration);
    }

    public void Close()
    {
        _blocker.enabled = false;
        _targetPos += Vector3.down * _moveY;
        transform.DOMoveY(_targetPos.y, _duration);
    }
}
