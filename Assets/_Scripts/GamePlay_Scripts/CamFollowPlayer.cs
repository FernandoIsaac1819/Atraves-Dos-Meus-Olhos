using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CamFollowPlayer : MonoBehaviour
{
    [SerializeField] float m_SmoothTime;

    public Vector3 m_Offset;
    private Vector3 m_Velocity = Vector3.zero;

    private void FixedUpdate()
    {
        FollowPlayer();

    }

    private void FollowPlayer() 
    {
        if (PlayerMovement.Instance == null) return;

        Vector3 desiredPosition = PlayerMovement.Instance.transform.position + m_Offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref m_Velocity, m_SmoothTime);
        transform.position = smoothedPosition;
    }
}
