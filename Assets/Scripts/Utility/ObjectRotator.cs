using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] Vector3 _axis;

    private void Update()
    {
        transform.Rotate(_axis, _speed * Time.deltaTime);
    }
}
