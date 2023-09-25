using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HandleInputs : MonoBehaviour
{
    private GameInputActions m_GameInputActions;
    public static HandleInputs Instance { get; private set; }

    // MOVEMENT DIRECTION CALCULATION
    private Transform m_Cam;
    private Vector3 m_CamForward;
    private Vector2 m_TargetMoveInput;
    private Vector2 m_CurrentMoveInput;
    private Vector2 m_SmoothInputVelocity;
    private Vector3 m_MovementDirection;
    private float m_SmoothInputSpeed;

    // CAMERA CONTROL
    private Vector3 m_CurrentCamInput;
    private Vector3 m_TargetCamInput;

    public EventHandler OnInteractPressed;
    public EventHandler OnInteractReleased;


    void Awake()
    {
        AssignCameraTransform();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        m_GameInputActions = new GameInputActions();
        m_GameInputActions.Player.Enable();

        m_GameInputActions.Player.Interact.performed += OnInteract_Performed;
        m_GameInputActions.Player.Interact.canceled += OnInteract_Canceled;

    }

    private void OnInteract_Canceled(InputAction.CallbackContext context)
    {
        OnInteractReleased?.Invoke(this, EventArgs.Empty);
    }

    private void OnInteract_Performed(InputAction.CallbackContext context)
    {
        OnInteractPressed?.Invoke(this, EventArgs.Empty);
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

        m_CurrentMoveInput = Vector2.SmoothDamp(m_CurrentMoveInput, m_TargetMoveInput, ref m_SmoothInputVelocity, m_SmoothInputSpeed);

        Vector3 direction = new Vector3(m_CurrentMoveInput.x, 0, m_CurrentMoveInput.y);

        if (m_Cam != null)
        {
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_MovementDirection = (direction.z * m_CamForward) + direction.x * m_Cam.right;
        }
        else
        {
            m_MovementDirection = direction;
        }

        m_MovementDirection.Normalize();

        return m_MovementDirection;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignCameraTransform();
    }


    private void AssignCameraTransform()
    {
        m_Cam = Camera.main?.transform;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}