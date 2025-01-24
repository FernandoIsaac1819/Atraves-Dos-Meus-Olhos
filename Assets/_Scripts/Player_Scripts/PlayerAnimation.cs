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
      
    void Start()
    {
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_Animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleAnimations();
    }

    /// <summary>
    /// Handles the animation of the player
    /// </summary>
    void HandleAnimations() 
    {
        m_Animator.applyRootMotion = true; 

        if(!m_PlayerMovement.IsGrounded) 
        {
            m_Animator.SetFloat(AnimationHashCodes.Instance.m_AirbornSpeedHash, m_PlayerMovement.Rigidbody.velocity.y);
        }

        m_Animator.SetBool(AnimationHashCodes.Instance.m_IsMovingHash, HandleInputs.Instance.IsMoving());
        m_Animator.SetBool(AnimationHashCodes.Instance.m_IsRunningHash, PlayerMovement.Instance.IsRunning);
        m_Animator.SetBool(AnimationHashCodes.Instance.m_GroundedHash, PlayerMovement.Instance.IsGrounded);
        m_Animator.SetFloat(AnimationHashCodes.Instance.m_TurnHashHash, PlayerMovement.Instance.TurnAmount * PlayerMovement.Instance.MovementSpeed, 0.1f, Time.deltaTime);
        m_Animator.SetFloat(AnimationHashCodes.Instance.m_ForwardHash , PlayerMovement.Instance.ForwardAmount * PlayerMovement.Instance.MovementSpeed, 0.1f, Time.deltaTime);
    }

}
