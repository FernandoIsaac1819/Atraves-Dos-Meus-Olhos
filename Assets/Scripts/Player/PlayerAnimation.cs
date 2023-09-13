using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator m_Animator;
    private PlayerMovement m_PlayerMovement;
    private Transformation m_Transformation;
    [SerializeField] private Avatar m_CatAvatar;
    [SerializeField] private Avatar m_HumanAvatar;
    public bool m_UseRootMotion;

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

    void HandleAnimations() 
    {
        m_Animator.applyRootMotion = true; 

        if(!m_PlayerMovement.IsGrounded) 
        {
            m_Animator.SetFloat(m_AirbornSpeedHash, m_PlayerMovement.Rigidbody.velocity.y);
        }

        m_Animator.SetBool(m_IsMovingHash, HandleInputs.Instance.IsMoving());
        m_Animator.SetBool(m_IsRunningHash, HandleInputs.Instance.IsRunPressed());
        m_Animator.SetBool(m_IsHumanHash, m_Transformation.IsHuman);
        m_Animator.SetBool(m_GroundedHash, m_PlayerMovement.IsGrounded);

        m_Animator.SetFloat(m_TurnHashHash, m_PlayerMovement.TurnAmount, 0.1f, Time.deltaTime);
        m_Animator.SetFloat(m_ForwardHash , m_PlayerMovement.ForwardAmount * m_PlayerMovement.MovementSpeed, 0.1f, Time.deltaTime);
    }

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
