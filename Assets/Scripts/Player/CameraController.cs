using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(0.01f, 0.5f)]
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private bool enableShaky = false;
    [SerializeField] private float maxAngle = 10f;
    [SerializeField] private float maxOffset = 0.5f;

    private float cameraAngle = 0.0f;
    private Vector3 cameraPosition;
    private float time = 0.0f;

    private void Start()
    {
        cameraAngle = transform.rotation.eulerAngles.z;
        cameraPosition = PlayerManager.Instance.GetPlayerPosition();
        cameraPosition.z = -10.0f;
    }

    private void FixedUpdate()
    {

        time += Time.fixedDeltaTime;

        Vector3 playerPos = PlayerManager.Instance.GetPlayerPosition();
        playerPos.z = cameraPosition.z;
        cameraPosition += (playerPos - cameraPosition) * speed;

        float angle = 0.0f;
        Vector3 offset = Vector3.zero;
        if (enableShaky && GlobalManager.Instance != null)
        {
            float trauma = GlobalManager.Instance.IsAlarmActivated() ? 0.5f : 0.2f; 

            float shake = trauma * trauma;

            angle = maxAngle * shake * (Mathf.PerlinNoise(1.1f, time) * 2.0f - 1.0f);
            offset.x = maxOffset * shake * (Mathf.PerlinNoise(3.3f, time) * 2.0f - 1.0f);
            offset.y = maxOffset * shake * (Mathf.PerlinNoise(5.5f, time) * 2.0f - 1.0f);
        }

        transform.position = cameraPosition + offset;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, cameraAngle + angle);
    }
}
