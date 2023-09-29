using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This controls animations relative to the player
/// </summary>
public class PlayerAnimation : MonoBehaviour
{
    private Animator m_Animator;
    private PlayerMovement m_PlayerMovement;
    private Transformation m_Transformation;
    
    [SerializeField] private Avatar m_CatAvatar;
    [SerializeField] private Avatar m_HumanAvatar;

    //ANIMATOR HASH CODES
    readonly int m_ForwardHash = Animator.StringToHash("Forward");
    readonly int m_TurnHashHash = Animator.StringToHash("Turn");
    private int m_GroundedHash = Animator.StringToHash("OnGround");
    readonly int m_AirbornSpeedHash = Animator.StringToHash("AirbornSpeed");
    readonly int m_IsHumanHash = Animator.StringToHash("IsHuman");
    readonly int m_IsRunningHash = Animator.StringToHash("IsRunning");
    readonly int m_IsMovingHash = Animator.StringToHash("IsMoving");
    

    void Start()
    {
        m_Transformation = GetComponentInParent<Transformation>();
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_Animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleAnimations();
        SwitchAvatars();
    }

    /// <summary>
    /// Handles the animation of the player
    /// </summary>
    void HandleAnimations() 
    {
        m_Animator.applyRootMotion = true; 

        if(!m_PlayerMovement.IsGrounded) 
        {
            m_Animator.SetFloat(m_AirbornSpeedHash, m_PlayerMovement.Rigidbody.velocity.y);
        }

        m_Animator.SetBool(m_IsMovingHash, HandleInputs.Instance.IsMoving());
        m_Animator.SetBool(m_IsRunningHash, PlayerMovement.Instance.IsRunning);
        m_Animator.SetBool(m_IsHumanHash, Transformation.Instance.IsHuman);
        m_Animator.SetBool(m_GroundedHash, PlayerMovement.Instance.IsGrounded);

        m_Animator.SetFloat(m_TurnHashHash, PlayerMovement.Instance.TurnAmount * PlayerMovement.Instance.MovementSpeed, 0.1f, Time.deltaTime);
        m_Animator.SetFloat(m_ForwardHash , PlayerMovement.Instance.ForwardAmount * PlayerMovement.Instance.MovementSpeed, 0.1f, Time.deltaTime);
    }

    /// <summary>
    /// Switches the avatar of the animator component depending on the current form of the character
    /// </summary>
    void SwitchAvatars() 
    {
        if(m_Transformation.IsHuman) 
        {
            m_Animator.avatar = m_HumanAvatar;
        } else 
        {
            m_Animator.avatar = m_CatAvatar;
        }
    }

}
