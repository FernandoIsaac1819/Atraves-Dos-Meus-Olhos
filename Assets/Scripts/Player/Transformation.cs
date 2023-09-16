using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformation : MonoBehaviour
{
    [SerializeField] private Animator m_TransformationAnimator;

    [Header("Forms")]
    [SerializeField] private GameObject m_HumanForm;
    [SerializeField] private GameObject m_CatForm;
    [SerializeField] private Collider m_HumanCollider;
    [SerializeField] private Collider m_CatCollider;


    [Header("Transformation variables")]
    [SerializeField] public bool m_IsHuman = true;
    [SerializeField] private bool m_CanTransform = true;
    [SerializeField] private float m_TransformationCoolDown;
    [SerializeField] private float m_SwitchFormTimer;
    public bool IsHuman {get{return m_IsHuman;} set {m_IsHuman = value;}}
    public bool CanTransform {get{return m_CanTransform;} set {m_CanTransform = value;}}

    void Start() 
    {
        m_CatForm.SetActive(false);
        m_HumanForm.SetActive(true);

        m_HumanCollider.enabled = true;
        m_CatCollider.enabled = false;
    }

    void Update() 
    {

        if(HandleInputs.Instance.IsTransformedPressed() && m_CanTransform) 
        {
            StartCoroutine(SwitchForms(m_SwitchFormTimer));
            StartCoroutine(TransformationCoolDown(m_TransformationCoolDown)); 
        }
    }

    IEnumerator SwitchForms(float time) 
    {
        if(m_IsHuman) 
        {
            m_TransformationAnimator.SetTrigger("Cat");
            m_CatCollider.enabled = true;
            m_HumanCollider.enabled = false;
            yield return new  WaitForSeconds(time);
            m_IsHuman = false;
        } 
        else
        {
            m_TransformationAnimator.SetTrigger("Human");
            m_HumanCollider.enabled = true;
            m_CatCollider.enabled = false;
            yield return new  WaitForSeconds(time);
            m_IsHuman = true;
        }
    }

    IEnumerator TransformationCoolDown(float time) 
    {
        m_CanTransform = false;
        yield return new WaitForSeconds(time);
        m_CanTransform = true;
    }
}
