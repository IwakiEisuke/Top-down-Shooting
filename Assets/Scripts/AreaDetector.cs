using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaDetector : MonoBehaviour
{
    [SerializeField] List<GameObject> _detectObjects;
    [SerializeField] UnityEvent _allClearedEvent;
    bool _activated;

    private void Update()
    {
        foreach (GameObject obj in _detectObjects)
        {
            if (obj && obj.activeInHierarchy == true)
            {
                return;
            }
        }

        if (!_activated)
        {
            Debug.Log($"{name} is all cleared");
            _allClearedEvent.Invoke();
            _activated = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_detectObjects.Contains(other.gameObject))
        {
            _detectObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_detectObjects.Contains(other.gameObject) || _detectObjects.Contains(other.transform.parent.gameObject))
        {
            _detectObjects.Remove(other.gameObject);
        }
    }
}
