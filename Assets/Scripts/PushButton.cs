using UnityEngine;
using UnityEngine.Events;

public class PushButton : MonoBehaviour
{
    [SerializeField] UnityEvent OnTriggerEnterEvent;
    bool isPushed;

    private void OnTriggerEnter(Collider other)
    {
        if (!isPushed)
        {
            OnTriggerEnterEvent.Invoke();
            isPushed = true;
        }
    }
}
