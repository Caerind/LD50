using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCameraComponent : MonoBehaviour
{
    [SerializeField] private float minAngle;
    [SerializeField] private float maxAngle;
    [SerializeField] private float fieldAngle = 45.0f;
    [SerializeField] private float fieldRadius = 7.0f;
    [SerializeField] private float rotSpeed = 20.0f;
    [SerializeField] private float timeWait = 3.0f;

    private bool goToMin = true;
    private float timer = 0.0f;
    private Vector2 dir;
    private float angle = 0.0f;
    private float startAngle = 0.0f;
    private int skipFrame;
    private static int skipFrameCount = 4;

    private void Start()
    {
        startAngle = transform.rotation.eulerAngles.z + 90.0f;
        skipFrame = gameObject.GetInstanceID() % skipFrameCount;
    }

    private void Update()
    {
        if (GlobalManager.Instance.IsAlarmActivated())
        {
            GameObject nearestPlayer = PlayerManager.Instance.GetNearestPlayer(transform.position);
            if (nearestPlayer != null)
            {
                Vector2 diff = (nearestPlayer.transform.position - transform.position);
                dir = diff.normalized;
                Look();
            }
        }
        else
        {
            skipFrame = (skipFrame + 1) % skipFrameCount;
            if (skipFrame == 0)
            {
                // Player detection
                if (PlayerManager.Instance.DetectPlayer(transform.position, dir, fieldRadius, fieldAngle, 0.0f))
                {
                    GlobalManager.Instance.ActivateAlarm();
                }
            }

            float dt = Time.deltaTime;

            if (timer > 0.0f && timer - dt <= 0.0f)
                AudioManager.PlaySound("camera");

            timer -= dt;

            if (timer < 0.0f)
            {
                if (goToMin)
                {
                    angle -= rotSpeed * dt;

                    float rad = (startAngle + angle) * Mathf.Deg2Rad;
                    dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
                    Look();

                    if (angle < minAngle)
                    {
                        AudioManager.StopSound("camera");
                        timer = timeWait;
                        goToMin = false;
                    }
                }
                else
                {
                    angle += rotSpeed * dt;

                    float rad = (startAngle + angle) * Mathf.Deg2Rad;
                    dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
                    Look();

                    if (angle > maxAngle)
                    {
                        AudioManager.StopSound("camera");
                        timer = timeWait;
                        goToMin = true;
                    }
                }
            }
        }
    }

    private void Look()
    {
        float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotZ - 90.0f);
    }
}
