using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagComponent : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float force = 16.0f;

    private Vector2 dir;
    [DisplayOnly] private float playerInput;
    private float t;
    [SerializeField] private float timer = 0.5f;

    [SerializeField] private float maxBonusScaleFactor = 0.3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void FromStrongbox()
    {
        rb.bodyType = RigidbodyType2D.Static;
    }

    public void FromThrow(Vector3 position, Vector2 dir, float playerInput)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        transform.position = position;
        this.dir = dir;
        this.playerInput = playerInput;
        t = timer;
    }

    private void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        t -= dt;

        if (t > 0.0f)
        {
            float max = 0.5f; // ?timer?
            float x = 180.0f * t / max;
            float s = Mathf.Sin(x * Mathf.Deg2Rad) * maxBonusScaleFactor;

            rb.MovePosition(rb.position + playerInput * force * dir * dt);
            transform.localScale = new Vector3(1 + s, 1 + s, 1 + s);

            if (t - dt < 0.0f)
            {
                rb.bodyType = RigidbodyType2D.Static;
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    public void Carry()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<ZoneTrigger>() != null)
        {
            GlobalManager.Instance.ResetLevel();
            GlobalManager.Instance.currentLevel++;
            PlayerManager.Instance.ResetPlayersSpawned();
            StateManager.Instance.SwitchToScene(0);
            AudioManager.PlaySound("win");
        }
    }
}

