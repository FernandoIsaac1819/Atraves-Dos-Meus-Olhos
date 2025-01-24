using UnityEngine;

public class CamFollowPlayer : MonoBehaviour
{
    //A target for the player camera which follows the player with an offset and smooth

    [SerializeField] float m_SmoothTime;
    [SerializeField] private Vector3 m_Offset;
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
