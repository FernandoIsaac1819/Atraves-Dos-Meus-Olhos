using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }
    private Rigidbody m_Rigidbody;
    public static Animator m_Animator;
    

    [Header("Movement")]
    [SerializeField] private float m_MovementSpeed = 5f;
    [SerializeField] private float m_RotationSpeed = 10f;

    private Vector3 m_MoveDirection;
    private float m_TurnAmount;
    private float m_ForwardAmount;
    private bool m_IsRunning;

    [Header("Grounded")]
    [SerializeField] private bool m_IsGrounded;
    [SerializeField] private float m_OrigGroundCheckDistance = 0.1f;
    [SerializeField] private float m_GroundCheckDistance = 0.1f;
    private Vector3 m_GroundNormal;

    [Header("Jump")]
    [SerializeField] private bool m_JumpReset = true;
    [SerializeField] private float m_AirControlAmount = 0.5f;
    [SerializeField] private float m_JumpCoolDown = 0.5f;
    [SerializeField] private float m_JumpPower = 5f;
    [SerializeField] private float extraTurnSpeed = 1;
    private bool m_Jump;

    [Header("Custom Gravity")]
    [SerializeField] private float fallMultiplier = 2.5f;

    // GETTERS AND SETTERS
    public Rigidbody Rigidbody { get { return m_Rigidbody; } set { m_Rigidbody = value; } }

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
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();

        HandleInputs.Instance.OnJumpPressed += OnJump_Pressed;
        HandleInputs.Instance.OnJumpReleased += OnJump_Released;

        UpdateFormParameters(TransformationManager.currentForm);
    }

    void Update()
    {
        HandleAnimations();
    }

    void FixedUpdate()
    {
        CheckGroundedStatus();

        if (m_IsGrounded)
        {
            HandlePlayerLocomotion();
            ApplyTurnRotation();

            if (m_Jump)
            {
                Jump();
            }
        }
        else
        {
            AirbornMovement();
            ApplyCustomGravity();
        }
    }

    /// <summary>
    /// Applies turn rotation while grounded
    /// </summary>
    void ApplyTurnRotation()
    {
        float turnSpeed = Mathf.Lerp(m_RotationSpeed, m_RotationSpeed, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }

    /// <summary>
    /// Handles movement and rotation when grounded
    /// </summary>
    void HandlePlayerLocomotion()
    {
        Vector3 input = HandleInputs.Instance.GetMovementDirection();

        // Convert input to local space
        m_MoveDirection = transform.InverseTransformDirection(input);
        m_MoveDirection = Vector3.ProjectOnPlane(m_MoveDirection, m_GroundNormal);

        // Calculate forward and turn amounts
        m_TurnAmount = Mathf.Atan2(m_MoveDirection.x, m_MoveDirection.z);
        m_ForwardAmount = m_MoveDirection.z;

        // Apply movement
        Vector3 worldSpaceMovement = transform.TransformDirection(new Vector3(0, 0, m_ForwardAmount)) * m_MovementSpeed * Time.deltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position + worldSpaceMovement);
    }

    /// <summary>
    /// Handles midair movement and rotation
    /// </summary>
    void AirbornMovement()
    {
        Vector3 input = HandleInputs.Instance.GetMovementDirection();
        Vector3 airForce = new Vector3(input.x * m_AirControlAmount, 0, input.z * m_AirControlAmount);

        // Apply movement force while midair
        m_Rigidbody.AddForce(airForce, ForceMode.VelocityChange);

        if (airForce.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(airForce.normalized, Vector3.up);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * extraTurnSpeed);      
        }
    }

    /// <summary>
    /// Applies custom gravity for falling
    /// </summary>
    void ApplyCustomGravity()
    {
        if (m_Rigidbody.velocity.y < 0)
        {
            m_Rigidbody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    /// <summary>
    /// Updates player parameters based on the transformation form
    /// </summary>
    public void UpdateFormParameters(TransformationBase_SO form)
    {
    m_MovementSpeed = form.walkingspeed;
    m_JumpPower = form.jumpPower;
    m_AirControlAmount = form.air_control_amount;
    m_JumpCoolDown = form.jump_cooldown;
    fallMultiplier = form.fallMultiplier;
    m_RotationSpeed = form.move_turning_speed;

    // m_Animator.avatar = form.avatar;
    m_Animator.Rebind();
    m_Animator.Update(0);
    }

    /// <summary>
    /// Handles jump functionality
    /// </summary>
    void Jump()
    {
        if (!m_IsGrounded) return;

        m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
        m_IsGrounded = false;
    }

    /// <summary>
    /// Checks if the player is grounded
    /// </summary>
    void CheckGroundedStatus()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hitInfo, m_GroundCheckDistance))
        {
            m_GroundNormal = hitInfo.normal;
            m_IsGrounded = true;
        }
        else
        {
            m_GroundNormal = Vector3.up;
            m_IsGrounded = false;
        }
    }

    /// <summary>
    /// Handles animations based on movement and state
    /// </summary>
    void HandleAnimations()
    {
        m_Animator.applyRootMotion = true;

        if (!m_IsGrounded)
        {
            m_Animator.SetFloat(AnimationHashCodes.Instance.AirbornSpeed, Rigidbody.velocity.y);
        }

        m_Animator.SetBool(AnimationHashCodes.Instance.IsGrounded, m_IsGrounded);
        m_Animator.SetFloat(AnimationHashCodes.Instance.Turn, m_TurnAmount, 0.1f, Time.deltaTime);
        m_Animator.SetFloat(AnimationHashCodes.Instance.Forward, m_ForwardAmount, 0.1f, Time.deltaTime);
    }

    private void OnJump_Pressed(object sender, EventArgs e)
    {
        m_Jump = true;
    }

    private void OnJump_Released(object sender, EventArgs e)
    {
        m_Jump = false;
    }
}
