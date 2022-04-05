using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIComponent : MonoBehaviour
{
    private List<Vector3> patrolPathNodes = new List<Vector3>();
    private int patrolPathNodeIndex = 0;

    private Rigidbody2D rb;

    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float speedAlarm = 5.0f;
    [SerializeField] private float nodeMinDistance = 0.5f;
    [SerializeField] private float fieldAngle = 45.0f;
    [SerializeField] private float fieldRadius = 7.0f;
    [SerializeField] private float tooClose = 3.5f;


    private Vector2 dir;
    private int skipFrame;
    private static int skipFrameCount = 4;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        skipFrame = gameObject.GetInstanceID() % skipFrameCount;
    }

    private void Update()
    {
        if (!GlobalManager.Instance.IsAlarmActivated())
        {
            skipFrame = (skipFrame + 1) % skipFrameCount;
            if (skipFrame == 0)
            {
                // Player detection
                if (PlayerManager.Instance.DetectPlayer(transform.position, dir, fieldRadius, fieldAngle, tooClose))
                {
                    GlobalManager.Instance.ActivateAlarm();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 movement = Vector2.zero;

        if (GlobalManager.Instance.IsAlarmActivated())
        {
            GameObject nearestPlayer = PlayerManager.Instance.GetNearestPlayer(transform.position);
            if (nearestPlayer != null)
            {
                Vector2 pPos = nearestPlayer.transform.position;
                Vector2 pOldPos = pPos - nearestPlayer.GetComponent<PlayerController>().lastValidMoveInput;
                Vector2 t = (pPos + pOldPos) *0.5f;

                Vector2 p = transform.position;
                Vector2 diff = (t - p);
                dir = diff.normalized;

                float a = Random.Range(0.0f, 0.4f);
                float b = Random.Range(-1.0f, -1.0f);
                float c = Random.Range(-1.0f, -1.0f);

                movement = dir + a * new Vector2(b, c);
                movement.Normalize();

                Look();
            }
        }
        else
        {
            Vector2 pos = transform.position;
            Vector2 currentNodePos = patrolPathNodes[patrolPathNodeIndex];
            Vector2 diff = (currentNodePos - pos);

            if (diff.sqrMagnitude < nodeMinDistance * nodeMinDistance)
            {
                patrolPathNodeIndex = (patrolPathNodeIndex + 1) % patrolPathNodes.Count;
                currentNodePos = patrolPathNodes[patrolPathNodeIndex];
            }

            dir = (currentNodePos - pos).normalized;

            movement = dir;

            Look();
        }

        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime * GetSpeed());
    }

    private void Look()
    {
        float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotZ - 90.0f);
    }

    public float GetSpeed()
    {
        return (GlobalManager.Instance.IsAlarmActivated()) ? speedAlarm : speed;
    }

    public void AddPathNode(Vector3 node)
    {
        patrolPathNodes.Add(node);
    }
}
