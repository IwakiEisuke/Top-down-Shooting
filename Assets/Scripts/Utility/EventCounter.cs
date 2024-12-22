using UnityEngine;
using UnityEngine.Events;

public class EventCounter : MonoBehaviour
{
    [Header("")]
    [SerializeField] int _targetCount;
    [SerializeField] UnityEvent _onReachedTargetEvent;
    int _currentCount;

    public void Call()
    {
        _currentCount++;
        if (_currentCount == _targetCount)
        {
            _onReachedTargetEvent.Invoke();
        }
    }
}
