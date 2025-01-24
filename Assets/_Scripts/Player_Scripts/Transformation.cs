using System;
using System.Collections;
using UnityEngine;
/// <summary>
/// The transformation mechanic of the player
/// </summary>
public class Transformation : MonoBehaviour
{
    public static Transformation Instance;

    [SerializeField] private Animator m_TransformationAnimator;

    [Header("Forms")]
    [SerializeField] private GameObject m_HumanForm;

    [Header("Transformation variables")]
    [SerializeField] public bool m_IsHuman = true;
    [SerializeField] private bool m_CanTransform = true;
    [SerializeField] private float m_TransformationCoolDown;
    [SerializeField] private float m_SwitchFormTimer;

    private bool m_Transform = false;
    public bool IsHuman {get{return m_IsHuman;} set {m_IsHuman = value;}}
    public bool CanTransform {get{return m_CanTransform;} set {m_CanTransform = value;}}

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
        HandleInputs.Instance.OnTransformPressed += OnTransform_Pressed;
        HandleInputs.Instance.OnTransformReleased += OnTransform_Released;
    }

    private void OnTransform_Released(object sender, EventArgs e)
    {
        m_Transform = false;
    }

    private void OnTransform_Pressed(object sender, EventArgs e)
    {
        m_Transform = true;
    }

    void Update() 
    {
        if(m_Transform /*HandleInputs.Instance.IsTransformedPressed()*/ && m_CanTransform) 
        {
            StartCoroutine(SwitchForms(m_SwitchFormTimer));
            StartCoroutine(TransformationCoolDown(m_TransformationCoolDown)); 
        }
    }

    /// <summary>
    /// Switches from cat to human and vice versa, switches the colliders and triggers transformation animations
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator SwitchForms(float time) 
    {
        if(m_IsHuman) 
        {
            m_TransformationAnimator.SetTrigger("Cat");

            yield return new  WaitForSeconds(time);
            m_IsHuman = false;
        } 
        else
        {
            m_TransformationAnimator.SetTrigger("Human");

            yield return new  WaitForSeconds(time);
            m_IsHuman = true;
        }
    }

    /// <summary>
    /// Controls the transformation cool down
    /// </summary>
    /// <param name="time"> The amount of time passed before the player can transform again</param>
    /// <returns></returns>
    IEnumerator TransformationCoolDown(float time) 
    {
        m_CanTransform = false;
        yield return new WaitForSeconds(time);
        m_CanTransform = true;
    }
}
