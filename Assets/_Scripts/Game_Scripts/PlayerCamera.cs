using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This is script is meant to change the camera so that it has a different focus or angle on the player when they transform
/// </summary>

public class PlayerCameraController : MonoBehaviour
{
    
    public static PlayerCameraController Instance; 

    [SerializeField] private CinemachineFreeLook m_PlayerCamera;
    [SerializeField] private Transform m_CameraTarget;
    [SerializeField] private float m_XCameraSensitivity;
    [SerializeField] private float m_YCameraSensitivity;
    [SerializeField] float m_SmoothTime;
    [SerializeField] private Vector3 m_Offset;
    private Vector3 m_Velocity = Vector3.zero;

    private float m_TransformSpeed = 1.5f;
    private float m_CurrentMiddleRigHeight;
    private float m_CurrentMiddleRigRadius;
    private float m_VelocityHeight;
    private float m_VelocityRadius;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start() 
    {
        
    }

    void Update()
    {
        CameraControl();
        FollowPlayer();
        //for control over the invert input
        //m_PlayerCamera.m_XAxis.m_InvertInput = false;
        //m_PlayerCamera.m_YAxis.m_InvertInput = false;
        //ChangeCamera();
    }

    /*
    void ChangeCamera() 
    {
        float targetHeight = Transformation.Instance.IsHuman ? m_HumanMiddleRigHeight : m_CatMiddleRigHeight;
        float targetRadius = Transformation.Instance.IsHuman ? m_HumanMiddleRigRadius : m_CatMiddleRigRadius;

        // Smoothly interpolate the current values towards the target values
        m_CurrentMiddleRigHeight = Mathf.SmoothDamp(m_CurrentMiddleRigHeight, targetHeight, ref m_VelocityHeight, m_TransformSpeed);
        m_CurrentMiddleRigRadius = Mathf.SmoothDamp(m_CurrentMiddleRigRadius, targetRadius, ref m_VelocityRadius, m_TransformSpeed);

        // Apply the interpolated values to the middle rig
        m_PlayerCamera.m_Orbits[1].m_Height = m_CurrentMiddleRigHeight;
        m_PlayerCamera.m_Orbits[1].m_Radius = m_CurrentMiddleRigRadius;
    }*/

    private void FollowPlayer() 
    {
        if(PlayerMovement.Instance == null) return;

        Vector3 desiredPosition = PlayerMovement.Instance.transform.position + m_Offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(m_CameraTarget.position, desiredPosition, ref m_Velocity, m_SmoothTime);
        m_CameraTarget.position = smoothedPosition;
    }

    void CameraControl() 
    {
        Vector2 camInput = HandleInputs.Instance.GetCameraInput();

        m_PlayerCamera.m_XAxis.Value += camInput.x * m_XCameraSensitivity;

        m_PlayerCamera.m_YAxis.Value += camInput.y * m_YCameraSensitivity;
    }

}
