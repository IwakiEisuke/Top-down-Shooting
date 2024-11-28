using UnityEngine;
using UnityEngine.AI;

public static class MyExtension
{
    public static void SetBoundsOnMesh(this Collider collider, GameObject gameObject, Vector3 offset = default)
    {
        GetRelativeBounds(gameObject, out var center, out var size, offset);

        switch (collider)
        {
            case BoxCollider box:
                box.center = center;
                box.size = size;
                break;
            case CapsuleCollider capsule:
                capsule.center = center;
                break;
            case SphereCollider sphere:
                sphere.center = center;
                sphere.radius = size.y / 2;
                break;
            case MeshCollider mesh:
                break;
            default:
                break;
        }
    }

    public static void SetBoundsOnMesh(this NavMeshObstacle obstacle, GameObject gameObject, Vector3 offset = default)
    {
        GetRelativeBounds(gameObject, out var center, out var size, offset);
        obstacle.center = center;
        obstacle.size = size;
    }

    /// <summary>
    /// Pivot���猩�����ΓI�ȃ��b�V�������ʒu�ƃT�C�Y��Ԃ��܂�
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="center"></param>
    /// <param name="size"></param>
    /// <param name="offset"></param>
    public static void GetRelativeBounds(this GameObject gameObject, out Vector3 center, out Vector3 size, Vector3 offset = default)
    {
        var renderer = gameObject.GetComponent<Renderer>();
        var targetMesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
        center = renderer.bounds.center - gameObject.transform.position; // pivot�ʒu�ƃ��f�����S�̃Y����␳
        center += offset; // ��]�̑O�ɑ���
        center = Quaternion.Inverse(gameObject.transform.rotation) * center; // ��]�̉e����ł�����
        if(targetMesh) size = targetMesh.bounds.size;
        else size = Vector3.zero;
    }
}
