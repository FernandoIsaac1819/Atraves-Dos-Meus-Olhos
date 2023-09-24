using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
/// <summary>
/// Everything related to player movement
/// </summary>

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance {get;private set;}
    private Rigidbody m_Rigidbody;
    private Transformation m_Transformation;

    [Header("Movement")]
    [SerializeField] private float m_CatWalkingSpeed;
    [SerializeField] private float m_CatRunningSpeed;
    [SerializeField] private float m_HumanWalkingSpeed;
    [SerializeField] private float m_HumanRunningSpeed;
    [SerializeField] private float m_MovingTurnSpeed;
    [SerializeField] private float m_StationaryTurnSpeed;
    private Vector3 m_MoveDirection;
    private float m_MovementSpeed;
    private float m_TurnAmount;
    private float m_ForwardAmount;

    [Header("Grounded")]
    [SerializeField] private bool m_IsGrounded;
    [SerializeField] private float m_OrigGroundCheckDistance;
    [SerializeField] private float m_GroundCheckDistance;
    private Vector3 m_GroundNormal;

    [Header("Jump")]
    [SerializeField] private bool m_JumpReset = true;
    [SerializeField] private float m_CatJumpPower;
    [SerializeField] private float m_HumanJumpPower;
    [SerializeField] private float m_AirControlAmount;
    [SerializeField] private float m_CatAirControl;
    [SerializeField] private float m_HumanAirControl;
    [SerializeField] private float m_JumpCoolDown;
    private float m_JumpPower;
    
    //GETTERS AND SETTERS
    public Rigidbody Rigidbody {get {return m_Rigidbody;} set {m_Rigidbody = value;}}
    public Vector3 MoveDirection {get{return m_MoveDirection;} set {m_MoveDirection = value;}}
    public bool IsGrounded {get{return m_IsGrounded;} set {m_IsGrounded = value;}}
    public bool CanJump {get{return m_JumpReset;} set {m_JumpReset = value;}}
    public float TurnAmount {get{return m_TurnAmount;} set {m_TurnAmount = value;}}
    public float JumpPower {get{return m_JumpPower;} set {m_JumpPower = value;}}
    public float ForwardAmount {get{return m_ForwardAmount;} set {m_ForwardAmount = value;}}
    public float MovementSpeed {get{return m_MovementSpeed;} set {m_MovementSpeed = value;}}

    void Awake() 
    {
        if(instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else if(instance != this) 
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        m_Transformation = GetComponentInParent<Transformation>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        CheckGroundedStatus();
        UpdateMovementParameters();

        if(m_IsGrounded) 
        {
            if(HandleInputs.Instance.IsMoving()) 
            {
                HandlePlayerLocomotion();
            } 
            else 
            {
                //simply stops the movement 
                m_ForwardAmount = 0;
                m_TurnAmount = 0;
            }

            if(HandleInputs.Instance.IsJumpPressed() && m_JumpReset) Jump(); 
        }    
        else 
        {
            ApplyExtraAirMovement();
            m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
        }
    }

    /// <summary>
    /// Handles the jump functionality using rigidbody
    /// </summary>
    void Jump() 
    {
        if(!m_JumpReset) {return;}

        m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
        m_GroundCheckDistance = 0.1f;
		m_IsGrounded = false;

        StartCoroutine(ResetCanJump(m_JumpCoolDown));
    }

    /// <summary>
    /// Resets the the ability to jump 
    /// </summary>
    /// <param name="jumpCoolDown">Decides the amount of time before jump is allowed</param>
    /// <returns></returns>
    private IEnumerator ResetCanJump(float jumpCoolDown) 
	{
        m_JumpReset = false;
        yield return new WaitForSeconds(jumpCoolDown);
		m_JumpReset = true;
	}

    /// <summary>
    /// Simply checks of the player is moving by checking the magnitude of the move direction
    /// </summary>
    /// <returns></returns>
    public bool IsMoving() 
    {
        return m_MoveDirection.magnitude > 0;
    }

    /// <summary>
    /// Handles the locomotion of the player
    /// </summary>
    void HandlePlayerLocomotion() 
    {
        m_MoveDirection = HandleInputs.Instance.GetMovementDirection();
        
        m_MoveDirection = transform.InverseTransformDirection(m_MoveDirection);

        m_MoveDirection = Vector3.ProjectOnPlane(m_MoveDirection, m_GroundNormal);

        m_TurnAmount = Mathf.Atan2(m_MoveDirection.x, m_MoveDirection.z);

        m_ForwardAmount = m_MoveDirection.z;

        ApplyExtraTurnRotation();

        Vector3 newMovementInput = new Vector3(0,0,m_MoveDirection.z);
        Vector3 movement = transform.TransformDirection(newMovementInput) * m_MovementSpeed * Time.deltaTime;
        
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement );
    }

    /// <summary>
    /// Checks the grounded state of the player
    /// </summary>
    void CheckGroundedStatus()
    {
        RaycastHit hitInfo;

		if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
		{
			m_GroundNormal = hitInfo.normal;
			m_IsGrounded = true; 
            m_Transformation.CanTransform = true;
		}
		else
		{
			m_IsGrounded = false;
			m_GroundNormal = Vector3.up;
            m_Transformation.CanTransform = false;
		}
	}

    /// <summary>
    /// Changes the movement parameters depending on the transformation state
    /// </summary>
    void UpdateMovementParameters() 
    {
        if(m_Transformation.IsHuman) 
        {
            m_MovementSpeed = m_HumanWalkingSpeed;
            m_JumpPower = m_HumanJumpPower;
            m_AirControlAmount = m_HumanAirControl;

            if(HandleInputs.Instance.IsRunPressed()) 
            {
                m_MovementSpeed = m_HumanRunningSpeed;
            }

        } else 
        {
            m_MovementSpeed = m_CatWalkingSpeed;
            m_JumpPower = m_CatJumpPower;
            m_AirControlAmount = m_CatAirControl;

            if(HandleInputs.Instance.IsRunPressed()) 
            {
                m_MovementSpeed = m_CatRunningSpeed;
            }
        }
    }

    /// <summary>
    /// Applies extra movement while the player is midair
    /// </summary>
    void ApplyExtraAirMovement() 
    {
        Vector3 AirMovementDirection = HandleInputs.Instance.GetMovementDirection();
        
        AirMovementDirection.Normalize();
        
        AirMovementDirection *= m_AirControlAmount;

        Vector3 ExtraAirMovement = new Vector3(AirMovementDirection.x, 0, AirMovementDirection.z);

        m_Rigidbody.AddForce(ExtraAirMovement, ForceMode.VelocityChange);
    }

    /// <summary>
    /// Adds extra rotation to the turn
    /// </summary>
    void ApplyExtraTurnRotation()
	{
		// help the character turn faster (this is in addition to root rotation in the animation)
		float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
		transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
	}

    /// <summary>
    /// Controls the XAxis of the cinemachine 
    /// </summary>
    
    /*
    void WalkUpStairs() 
    {
        RaycastHit HitLow;
        if(Physics.Raycast(m_StepUpLow.transform.position, transform.TransformDirection(Vector3.forward), out HitLow, m_StairDistanceLow)) 
        {
            RaycastHit HitHigh;
            if(!Physics.Raycast(m_StepUpLow.transform.position, transform.TransformDirection(Vector3.forward), out HitHigh, m_StairDistanceHigh)) 
            {
                m_Rigidbody.position -= new Vector3( 0, -m_StepUpSmooth , 0);
            }
        }

        RaycastHit HitLow45;
        if(Physics.Raycast(m_StepUpLow.transform.position, transform.TransformDirection( 1.5f, 0 , 1 ), out HitLow45, m_StairDistanceLow)) 
        {
            RaycastHit HitHigh45;
            if(!Physics.Raycast(m_StepUpLow.transform.position, transform.TransformDirection( 1.5f, 0 , 1 ), out HitHigh45, m_StairDistanceHigh)) 
            {
                m_Rigidbody.position -= new Vector3( 0, -m_StepUpSmooth , 0);
            }
        }

        RaycastHit HitLowMinus45;
        if(Physics.Raycast(m_StepUpLow.transform.position, transform.TransformDirection( -1.5f, 0 , 1 ), out HitLowMinus45, m_StairDistanceLow)) 
        {
            RaycastHit HitHighMinus45;
            if(!Physics.Raycast(m_StepUpLow.transform.position, transform.TransformDirection( -1.5f, 0 , 1 ), out HitHighMinus45, m_StairDistanceHigh)) 
            {
                m_Rigidbody.position -= new Vector3( 0, -m_StepUpSmooth , 0);
            }
        }
    }
    */

}
