#define CAMERA_DEBUG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    private Vector3 m_PlayerFocusPosition;
    private float m_PlayerCameraAngle;
    private Vector3 m_OtherFocusPosition;
    private bool m_IsFocusingPlayer = true;

    [SerializeField] private float horizontalSpeedFactor = 0.1f;
    [SerializeField] private float verticalSpeedFactor = 0.2f;

    public void SetPlayerFocusPosition(Vector3 playerFocusPosition)
    {
        m_PlayerFocusPosition = playerFocusPosition;
    }

    public void SetPlayerCameraAngle(float playerCameraAngle)
    {
        m_PlayerCameraAngle = playerCameraAngle;
    }

    public void SetOtherFocusPosition(Vector3 otherFocusPosition)
    {
        m_OtherFocusPosition = otherFocusPosition;
    }

    public void SetFocusPlayer(bool focusPlayer)
    {
        m_IsFocusingPlayer = focusPlayer;
    }

    public bool IsFocusingPlayer()
    {
        return m_IsFocusingPlayer;
    }

    private void FixedUpdate()
    {
        Vector3 target = (m_IsFocusingPlayer) ? m_PlayerFocusPosition : m_OtherFocusPosition;
        target.z = -10.0f;
        float timeScale = 1.0f;
        float dx = (target.x - transform.position.x) * horizontalSpeedFactor * timeScale;
        float dy = (target.y - transform.position.y) * verticalSpeedFactor * timeScale;
        transform.position += new Vector3(dx, dy, 0.0f);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, (m_IsFocusingPlayer) ? m_PlayerCameraAngle : 0.0f);
    }

#if CAMERA_DEBUG
    [SerializeField] private bool cameraDebug = false;

    private void OnDrawGizmos()
    {
        if (cameraDebug)
        {
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(m_PlayerFocusPosition, new Vector3(0, 0, 1), 0.1f);

            UnityEditor.Handles.color = Color.blue;
            UnityEditor.Handles.DrawWireDisc(transform.position + new Vector3(0.01f, 0.01f, 0.0f), new Vector3(0, 0, 1), 0.1f);

            UnityEditor.Handles.color = Color.black;
            UnityEditor.Handles.DrawWireDisc(m_OtherFocusPosition, new Vector3(0, 0, 1), 0.1f);
        }        
    }
#endif // TRAUMA_DEBUG
}
