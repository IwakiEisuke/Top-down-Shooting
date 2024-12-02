using UnityEngine;
using UnityEngine.Events;

public class TriggerHandler : MonoBehaviour
{
    [SerializeField] bool _oneShot = true;
    [SerializeField] UnityEvent OnTriggerEnterEvent;
    [SerializeField] UnityEvent OnTriggerExitEvent;
    bool _entered;
    bool _exited;

    private void OnTriggerEnter(Collider other)
    {
        if (!_entered)
        {
            OnTriggerEnterEvent.Invoke();
            _entered = _oneShot;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_exited)
        {
            OnTriggerExitEvent.Invoke();
            _exited = _oneShot;
        }
    }
}
