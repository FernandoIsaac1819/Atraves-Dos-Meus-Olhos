using System.Collections;
using System.Collections.Generic;
using DissolveExample;
using UnityEngine;

/// <summary>
/// Target for the cinemachine player camera to follow 
/// </summary>
public class CamFollowPlayer : MonoBehaviour
{

    public float m_SmoothTime;

    public Vector3 m_Offset;
    private Vector3 m_Velocity = Vector3.zero;

    private void FixedUpdate()
    {
        FollowPlayer();
    }


    private void FollowPlayer() 
    {
        if (PlayerMovement.instance == null) return;

        Vector3 desiredPosition = PlayerMovement.instance.transform.position + m_Offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref m_Velocity, m_SmoothTime);
        transform.position = smoothedPosition;
    }
}
