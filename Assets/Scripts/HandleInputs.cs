using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandleInputs : MonoBehaviour
{
    private GameInputActions m_GameInputActions;
    public static HandleInputs Instance {get; private set;}

    //MOVEMENT DIRECTION CALCULATION
    private Transform m_Cam;
    private Vector3 m_CamForward; 
    private Vector2 m_TargetMoveInput;
    private Vector2 m_CurrentMoveInput;
    private Vector2 m_SmoothInputVelocity;
    private Vector3 m_MovementDirection;
    private float m_SmoothInputSpeed;

    //CAMERA CONTROL
    private Vector3 m_CurrentCamInput;
    private Vector3 m_TargetCamInput;

    void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // This ensures that the instance isn't destroyed when loading a new scene.
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Ensures that there's only one instance of the script in the scene.
        }

        //set up input system    
        m_GameInputActions = new GameInputActions();
        m_GameInputActions.Player.Enable();

        //assign variables
        m_Cam = Camera.main.transform;
    }

    public bool IsEmotePressed() 
    {
        return m_GameInputActions.Player.Emote.IsPressed();
    }

    public bool IsJumpPressed() 
    {
        return m_GameInputActions.Player.Jump.IsPressed();
    }

    public bool IsTransformedPressed() 
    {
        return m_GameInputActions.Player.Transform.IsPressed();
    }

    public bool IsRunPressed() 
    {
        return m_GameInputActions.Player.Run.IsPressed();
    }

    public Vector2 GetCameraInput() 
    {
        m_CurrentCamInput = m_GameInputActions.Player.CameraController.ReadValue<Vector2>();
        return m_CurrentCamInput;
    }

    public bool IsMoving() 
    {
        Vector3 moveDir = GetMovementDirection();
        return moveDir.magnitude > 0;
    }

    public Vector3 GetMovementDirection() 
    {
        m_TargetMoveInput = m_GameInputActions.Player.Movement.ReadValue<Vector2>();

		m_CurrentMoveInput = Vector2.SmoothDamp(m_CurrentMoveInput, m_TargetMoveInput, ref m_SmoothInputVelocity, m_SmoothInputSpeed );

		Vector3 direction = new Vector3(m_CurrentMoveInput.x, 0, m_CurrentMoveInput.y);

        m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;

        m_MovementDirection = (direction.z * m_CamForward) + direction.x * m_Cam.right;

        m_MovementDirection.Normalize();

        return m_MovementDirection;
    }
}
