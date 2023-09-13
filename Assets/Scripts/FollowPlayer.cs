using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Tooltip("Target to follow")]
    public Transform m_Target;
    
    [Tooltip("Smoothness factor for following. Lower value means smoother following.")]
    public float m_SmoothTime;

    [Tooltip("Offset from the target")]
    public Vector3 m_Offset;

    private Vector3 m_Velocity = Vector3.zero;

    private void FixedUpdate()
    {
        if (m_Target == null) return;

        Vector3 desiredPosition = m_Target.position + m_Offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref m_Velocity, m_SmoothTime);
        transform.position = smoothedPosition;
    }
}
