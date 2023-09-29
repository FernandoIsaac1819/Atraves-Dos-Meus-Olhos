using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance; 
    [SerializeField] private CinemachineFreeLook m_PlayerCamera;
    [SerializeField] private float m_XCameraSensitivity;

    [Header("Human Settings")]
    [SerializeField] private float m_HumanMiddleRigHeight;
    [SerializeField] private float m_HumanMiddleRigRadius;

    [Header("Cat Settings")]
    [SerializeField] private float m_CatMiddleRigHeight;
    [SerializeField] private float m_CatMiddleRigRadius;

    [Header("Transformation")]

    [SerializeField] private float m_TransformSpeed;

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
        m_CurrentMiddleRigHeight = m_HumanMiddleRigHeight;
        m_CurrentMiddleRigRadius = m_HumanMiddleRigRadius;
    }

    void Update()
    {
        CameraControl();
        ChangeCamera();
    }

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
    }

    void CameraControl() 
    {
        Vector2 camInput = HandleInputs.Instance.GetCameraInput();
        m_PlayerCamera.m_XAxis.Value += camInput.x * m_XCameraSensitivity;
    }

}
