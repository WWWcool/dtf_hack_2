using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityPrototype;

[System.Serializable]
public class PlayerFollowContext
{

    public float lowerEdge = 100f;
}

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform m_target;

    [SerializeField] private float m_smoothSpeed = 0.125f;
    [SerializeField] private Vector3 m_offset;
    [SerializeField] private bool m_followPlayer = true;
    [SerializeField] private bool m_alwaysOnFieldHor = true;
    [SerializeField] private bool m_alwaysOnFieldVer = false;
    [SerializeField] private bool m_allowExceedTop = false;

    private Field m_field;
    private float m_horMin, m_horMax, m_VerMin, m_VerMax;

    void Start()
    {
        if (m_followPlayer)
        {
            var playerInputListener = ServiceLocator.FindObjectOfType<PlayerInputListener>();
            if (playerInputListener)
            {
                m_target = playerInputListener.transform;
            }
            m_field = ServiceLocator.FindObjectOfType<Field>();
        }

        if (m_alwaysOnFieldHor || m_alwaysOnFieldVer || !m_allowExceedTop)
        {
            var camera = this.GetCachedComponent<Camera>();
            var horSize = camera.orthographicSize * Screen.width / Screen.height;
            var fieldLeftEdge = m_field.transform.position.x - m_field.resolution.x * m_field.tileSize.x / 2;
            var fieldRightEdge = m_field.transform.position.x + m_field.resolution.x * m_field.tileSize.x / 2;
            m_horMin = fieldLeftEdge + horSize;
            m_horMax = fieldRightEdge - horSize;

            var verSize = camera.orthographicSize;
            var fieldButtomEdge = m_field.transform.position.y - m_field.resolution.y * m_field.tileSize.y / 2;
            var fieldTopEdge = m_field.transform.position.y + m_field.resolution.y * m_field.tileSize.y / 2;
            m_VerMin = fieldButtomEdge + verSize;
            m_VerMax = fieldTopEdge - verSize;
        }
    }

    void FixedUpdate()
    {
        Vector3 desiredPosition = m_target.position + m_offset;
        desiredPosition.z = transform.position.z;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, m_smoothSpeed);
        if (m_alwaysOnFieldHor)
        {
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, m_horMin, m_horMax);
        }
        if (!m_allowExceedTop)
        {
            if (smoothedPosition.y >= m_VerMax)
            {
                smoothedPosition.y = m_VerMax;
            }
        }
        if (m_alwaysOnFieldVer)
        {
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, m_VerMin, m_VerMax);
        }
        transform.position = smoothedPosition;
    }
}
