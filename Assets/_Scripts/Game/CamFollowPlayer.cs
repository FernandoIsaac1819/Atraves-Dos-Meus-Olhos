using System.Collections;
using System.Collections.Generic;
using DissolveExample;
using UnityEngine;

/// <summary>
/// Target for the cinemachine player camera to follow 
/// </summary>
public class CamFollowPlayer : MonoBehaviour
{
    /// <summary>
    /// The target to be followed
    /// </summary>
    public Transform m_Target;
    /// <summary>
    /// controls the speed of the follow 
    /// </summary>
    public float m_SmoothTime;
    /// <summary>
    /// the space between the target and the camera
    /// </summary>
    public Vector3 m_Offset;
    private Vector3 m_Velocity = Vector3.zero;

    private void FixedUpdate()
    {
        FollowPlayer();
    }

    /// <summary>
    /// This simply smoothly follows the target with an offset
    /// </summary>
    private void FollowPlayer() 
    {
        if (m_Target == null) return;

        Vector3 desiredPosition = m_Target.position + m_Offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref m_Velocity, m_SmoothTime);
        transform.position = smoothedPosition;
    }
}
