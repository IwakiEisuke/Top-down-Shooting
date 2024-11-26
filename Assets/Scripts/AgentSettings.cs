using UnityEngine;

public class AgentSettings : MonoBehaviour
{
    [SerializeField] float _linearDrag = 1;
    public float LinearDrag => _linearDrag;
}
