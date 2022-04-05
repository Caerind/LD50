using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointComponent : MonoBehaviour
{
    [SerializeField] private float size = 1.0f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, size);
    }
}
