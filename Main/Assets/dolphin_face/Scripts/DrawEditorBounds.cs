using UnityEngine;
using System.Collections;

public class DrawEditorBounds : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, collider.bounds.extents);
    }
}
