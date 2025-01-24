using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHashCodes : MonoBehaviour
{
    //Store all animation strings here for easy access and alteration
    public static AnimationHashCodes Instance {get; private set;}

    //Human character
    [HideInInspector] public int m_ForwardHash = Animator.StringToHash("Forward");
    [HideInInspector] public int m_TurnHashHash = Animator.StringToHash("Turn");
    [HideInInspector] public int m_GroundedHash = Animator.StringToHash("OnGround");
    [HideInInspector] public int m_AirbornSpeedHash = Animator.StringToHash("AirbornSpeed");
    [HideInInspector] public int m_IsHumanHash = Animator.StringToHash("IsHuman");
    [HideInInspector] public int m_IsRunningHash = Animator.StringToHash("IsRunning");
    [HideInInspector] public int m_IsMovingHash = Animator.StringToHash("IsMoving");

    void Awake() 
    {
        if(Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else if(Instance != this) 
        {
            Destroy(gameObject);
        }
    }
}
