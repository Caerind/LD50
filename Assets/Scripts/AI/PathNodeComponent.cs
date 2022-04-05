using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNodeComponent : MonoBehaviour
{
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
