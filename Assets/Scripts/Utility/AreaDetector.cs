using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaDetector : MonoBehaviour
{
    [SerializeField] List<GameObject> _detectObjects;
    [SerializeField] UnityEvent _allClearedEvent;
    bool _autoDetectionCompleted;
    bool _eventInvoked;

    private void Start()
    {
        var dummy = _detectObjects.ToArray();
        foreach (GameObject obj in dummy)
        {
            if (!obj.TryGetComponent<Collider>(out _))
            {
                _detectObjects.Add(obj.GetComponentInChildren<Collider>().gameObject);
                _detectObjects.Remove(obj);
            }
        }
    }

    private void Update()
    {
        if (_autoDetectionCompleted)
        {
            foreach (GameObject obj in _detectObjects)
            {
                if (obj && obj.activeInHierarchy == true)
                {
                    return;
                }
            }

            if (!_eventInvoked)
            {
                Debug.Log($"{name} is all cleared");
                _allClearedEvent.Invoke();
                _eventInvoked = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _autoDetectionCompleted = true;
        if (!_detectObjects.Contains(other.gameObject))
        {
            _detectObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_detectObjects.Contains(other.gameObject))
        {
            _detectObjects.Remove(other.gameObject);
        }
    }
}
