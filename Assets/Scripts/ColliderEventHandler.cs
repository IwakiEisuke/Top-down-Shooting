using UnityEngine;
using UnityEngine.Events;

public class ColliderEventHandler : MonoBehaviour
{
    [SerializeField] UnityEvent OnTriggerEnterEvent;
    bool _invoked;

    private void OnTriggerEnter(Collider other)
    {
        if (!_invoked)
        {
            OnTriggerEnterEvent.Invoke();
            _invoked = true;
        }
    }
}
