using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance; 
    [SerializeField] private CinemachineFreeLook m_PlayerCamera;
    [SerializeField] private float m_XCameraSensitivity;

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
        m_PlayerCamera = GetComponentInChildren<CinemachineFreeLook>();
    }


    void FixedUpdate()
    {
        CameraControl();
    }

    void CameraControl() 
    {
        Vector2 camInput = HandleInputs.Instance.GetCameraInput();
        m_PlayerCamera.m_XAxis.Value += camInput.x * m_XCameraSensitivity;
    }
}
