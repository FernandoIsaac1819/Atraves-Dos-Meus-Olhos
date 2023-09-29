using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HandleInputs : MonoBehaviour
{
    public static HandleInputs Instance { get; private set; }

    private GameInputActions m_PlayerInputActions;
    public  GameInputActions PlayerInputActions {get {return m_PlayerInputActions;} set {m_PlayerInputActions = value;}}

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

    public EventHandler OnEmotePressed;
    public EventHandler OnEmoteReleased;

    public EventHandler OnJumpPressed;
    public EventHandler OnJumpReleased;

    public EventHandler OnTransformPressed;
    public EventHandler OnTransformReleased;

    public EventHandler OnRunPressed;
    public EventHandler OnRunReleased;

    public EventHandler OnMapPressed;
    public EventHandler OnMapReleased;

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
    }

    void Start() 
    {
        m_PlayerInputActions = new GameInputActions();
        m_PlayerInputActions.Player.Enable();
        m_PlayerInputActions.UI.Enable();

        m_PlayerInputActions.Player.Run.performed += OnRun_Performed;
        m_PlayerInputActions.Player.Run.canceled += OnRun_Canceled;

        m_PlayerInputActions.Player.Interact.performed += OnInteract_Performed;
        m_PlayerInputActions.Player.Interact.canceled += OnInteract_Canceled;

        m_PlayerInputActions.Player.Emote.performed += OnEmote_Performed;
        m_PlayerInputActions.Player.Emote.canceled += OnEmote_Canceled;

        m_PlayerInputActions.Player.Jump.performed += OnJump_Performed;
        m_PlayerInputActions.Player.Jump.canceled += OnJump_Canceled;

        m_PlayerInputActions.Player.Transform.performed += OnTransform_Performed;
        m_PlayerInputActions.Player.Transform.canceled += OnTransform_Canceled;

        m_PlayerInputActions.UI.Menu.performed += OnMap_Performed;
        m_PlayerInputActions.UI.Menu.canceled += OnMap_Canceled;
    }

    private void OnMap_Canceled(InputAction.CallbackContext context)
    {
        OnMapReleased?.Invoke(this, EventArgs.Empty);
    }

    private void OnMap_Performed(InputAction.CallbackContext context)
    {
        OnMapPressed?.Invoke(this, EventArgs.Empty);
    }

    private void OnRun_Canceled(InputAction.CallbackContext context)
    {
        OnRunReleased?.Invoke(this, EventArgs.Empty);
    }

    private void OnRun_Performed(InputAction.CallbackContext context)
    {
        OnRunPressed?.Invoke(this, EventArgs.Empty);
    }

    private void OnTransform_Canceled(InputAction.CallbackContext context)
    {
        OnTransformReleased?.Invoke(this, EventArgs.Empty);
    }

    private void OnTransform_Performed(InputAction.CallbackContext context)
    {
        OnTransformPressed?.Invoke(this, EventArgs.Empty);
    }

    private void OnJump_Canceled(InputAction.CallbackContext context)
    {
        OnJumpReleased?.Invoke(this, EventArgs.Empty);
    }

    private void OnJump_Performed(InputAction.CallbackContext context)
    {
        OnJumpPressed?.Invoke(this, EventArgs.Empty);
    }

    private void OnEmote_Canceled(InputAction.CallbackContext context)
    {
        OnEmoteReleased?.Invoke(this, EventArgs.Empty);
    }

    private void OnEmote_Performed(InputAction.CallbackContext context)
    {
        OnEmotePressed?.Invoke(this, EventArgs.Empty);
    }

    private void OnInteract_Canceled(InputAction.CallbackContext context)
    {
        OnInteractReleased?.Invoke(this, EventArgs.Empty);
    }

    private void OnInteract_Performed(InputAction.CallbackContext context)
    {
        OnInteractPressed?.Invoke(this, EventArgs.Empty);
    }


    public bool IsRunPressed()
    {
        return m_PlayerInputActions.Player.Run.IsPressed();
    }

    public Vector2 GetCameraInput()
    {
        m_CurrentCamInput = m_PlayerInputActions.Player.CameraController.ReadValue<Vector2>();
        return m_CurrentCamInput;
    }

    public bool IsMoving()
    {
        Vector3 moveDir = GetMovementDirection();
        return moveDir.magnitude > 0;
    }


    public Vector3 GetMovementDirection()
    {
        m_TargetMoveInput = m_PlayerInputActions.Player.Movement.ReadValue<Vector2>();

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

    public Vector2 GetUINavigationVector() 
    {
        return m_PlayerInputActions.UI.Navigate.ReadValue<Vector2>();
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