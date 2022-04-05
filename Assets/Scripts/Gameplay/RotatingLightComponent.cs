using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingLightComponent : MonoBehaviour
{
    [SerializeField] private float rotSpeed = 360.0f;

    private void Update()
    {
        float dt = Time.deltaTime;
        float angle = transform.rotation.eulerAngles.z;
        angle += rotSpeed * dt;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }
}
