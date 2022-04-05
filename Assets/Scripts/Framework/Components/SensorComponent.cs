using UnityEngine;
using System.Collections.Generic;

public class SensorComponent : MonoBehaviour
{
    private List<Collider2D> m_Collisions = new List<Collider2D>();
    private float m_DisableTimer;

    public bool IsColliding()
    {
        return m_DisableTimer < 0.0f && m_Collisions.Count > 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger && !m_Collisions.Contains(other))
            m_Collisions.Add(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.isTrigger)
            m_Collisions.Remove(other);
    }

    private void Update()
    {
		m_DisableTimer -= Time.deltaTime;
    }

    public void Enable()
    {
        m_DisableTimer = -0.1f;
    }

    public void Disable(float duration)
    {
        m_DisableTimer = duration;
    }

    public List<Collider2D> GetCollisions()
    {
        return m_Collisions;
    }
}
