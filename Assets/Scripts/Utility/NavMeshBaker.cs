using Unity.AI.Navigation;
using UnityEngine;

[RequireComponent(typeof(NavMeshSurface))]
public class NavMeshBaker : MonoBehaviour
{
    NavMeshSurface _surface;

    private void Start()
    {
        _surface = GetComponent<NavMeshSurface>();
    }

    private void Update()
    {
        _surface.BuildNavMesh();
    }
}
