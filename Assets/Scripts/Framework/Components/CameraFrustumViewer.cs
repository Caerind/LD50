using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFrustumViewer : MonoBehaviour
{
    private Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    private void Update()
    {
        DrawCameraFrustum();
    }

    private void DrawCameraFrustum()
    {
        Vector3[] nearCorners = new Vector3[4]; // Approx'd nearplane corners
        Vector3[] farCorners = new Vector3[4]; // Approx'd farplane corners
        Plane[] camPlanes = GeometryUtility.CalculateFrustumPlanes(camera); // Get planes from matrix

        Plane temp = camPlanes[1]; camPlanes[1] = camPlanes[2]; camPlanes[2] = temp; // Swap [1] and [2] so the order is better for the loop
        for (int i = 0; i < 4; i++)
        {
            nearCorners[i] = Plane3Intersect(camPlanes[4], camPlanes[i], camPlanes[(i + 1) % 4]); // Near corners on the created projection matrix
            farCorners[i] = Plane3Intersect(camPlanes[5], camPlanes[i], camPlanes[(i + 1) % 4]); // Far corners on the created projection matrix
        }

        for (int i = 0; i < 4; i++)
        {
            Debug.DrawLine(nearCorners[i], nearCorners[(i + 1) % 4], Color.red, Time.deltaTime, true); // Near corners on the created projection matrix
            Debug.DrawLine(farCorners[i], farCorners[(i + 1) % 4], Color.blue, Time.deltaTime, true); // Far corners on the created projection matrix
            Debug.DrawLine(nearCorners[i], farCorners[i], Color.green, Time.deltaTime, true); // Sides of the created projection matrix
        }
    }

    private Vector3 Plane3Intersect(Plane p1, Plane p2, Plane p3)
    { 
        // Get the intersection point of 3 planes
        return ((-p1.distance * Vector3.Cross(p2.normal, p3.normal)) +
                (-p2.distance * Vector3.Cross(p3.normal, p1.normal)) +
                (-p3.distance * Vector3.Cross(p1.normal, p2.normal))) /
         (Vector3.Dot(p1.normal, Vector3.Cross(p2.normal, p3.normal)));
    }
}