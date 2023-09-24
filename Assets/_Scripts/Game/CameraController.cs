using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook m_PlayerCamera;
    [SerializeField] private float m_XCameraSensitivity;
    
    void Update()
    {
        Vector2 camInput = HandleInputs.Instance.GetCameraInput();
        m_PlayerCamera.m_XAxis.Value += camInput.x * m_XCameraSensitivity;
    }

}
