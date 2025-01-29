using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHashCodes : MonoBehaviour
{
    //Store all animation strings here for easy access and alteration
    public static AnimationHashCodes Instance {get; private set;}

    //Human character
    private string m_TransformInto = "TransformInto";
    private int m_ForwardHash = Animator.StringToHash("Forward");
    private int m_TurnHashHash = Animator.StringToHash("Turn");
    private int m_GroundedHash = Animator.StringToHash("OnGround");
    private int m_AirbornSpeedHash = Animator.StringToHash("AirbornSpeed");
    private int m_IsHumanHash = Animator.StringToHash("IsHuman");
    private int m_IsRunningHash = Animator.StringToHash("IsRunning");
    private int m_IsMovingHash = Animator.StringToHash("IsMoving");

    public string TransformInto {get{return m_TransformInto;} set{m_TransformInto = value;}}
    public int Forward {get{return m_ForwardHash;}set {m_ForwardHash = value;}}
    public int Turn {get{return m_TurnHashHash;}set {m_TurnHashHash = value;}}
    public int IsGrounded {get{return m_GroundedHash;}set {m_GroundedHash = value;}}
    public int AirbornSpeed {get{return m_AirbornSpeedHash;}set {m_AirbornSpeedHash = value;}}
    public int IsHuman {get{return m_IsHumanHash;}set {m_IsHumanHash = value;}}
    public int IsRunning {get{return m_IsRunningHash;}set {m_IsRunningHash = value;}}
    public int IsMoving {get{return m_IsMovingHash;}set {m_IsMovingHash = value;}}

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
