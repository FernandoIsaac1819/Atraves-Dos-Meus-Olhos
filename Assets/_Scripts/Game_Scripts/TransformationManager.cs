using System;
using System.Collections.Generic;
using UnityEngine;

public class TransformationManager : MonoBehaviour
{
    public static TransformationManager Instance;

    [SerializeField] private List<TransformationBase_SO> m_Transformations = new List<TransformationBase_SO>();
    [SerializeField] private TransformationBase_SO m_humanForm;

    private TransformationBase_SO m_selectedForm;
    private TransformationBase_SO m_currentForm;
    private int m_CurrentFormIndex = 0;
    private bool m_isHuman;
    public bool isHuman {get{return m_isHuman;} set{m_isHuman = value;}}   

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
        //sets the human form as default
        m_currentForm = m_humanForm;
        m_isHuman = true;
        PlayerMovement.Instance.UpdateFormParameter(m_currentForm);

        //means there is no forms at first
        m_selectedForm = null;
        
        HandleInputs.Instance.OnNextFormPressed += OnNextFormPressed;

        HandleInputs.Instance.OnTransformPressed += OnTransformPressed;

        HandleInputs.Instance.On_RevertHumanPressed += On_RevertHuman_Pressed;
        HandleInputs.Instance.On_RevertHumanCanceled += On_RevertHumanCanceled;
    }

    public void TransformInto(TransformationBase_SO form) 
    {
        if (form == null || m_currentForm == form) return;

        m_currentForm = form;

        m_currentForm.CopyFrom(form);

        m_isHuman = false;

        PlayerMovement.Instance.UpdateFormParameter(m_currentForm);

        Debug.Log("I am " + m_currentForm.name);
    }

    private TransformationBase_SO CycleThroughTransformations()
    {
        m_CurrentFormIndex = (m_CurrentFormIndex + 1) % m_Transformations.Count;

        m_selectedForm = m_Transformations[m_CurrentFormIndex];
        Debug.Log(m_CurrentFormIndex);
        return m_selectedForm;
        // Optionally update UI here
    }

    private void RevertToHuman() 
    {
        if(m_isHuman) return;

        m_currentForm = m_humanForm;

        m_currentForm.CopyFrom(m_humanForm);

        PlayerMovement.Instance.UpdateFormParameter(m_currentForm);

        Debug.Log("I AM HUMAN!");
    }

    public void AddNewTransformation(TransformationBase_SO newform) 
    {
        if(!m_Transformations.Contains(newform)) 
        {
            m_Transformations.Add(newform);
        }
    }

    private void On_RevertHuman_Pressed(object sender, EventArgs e)
    {
        RevertToHuman();
    }

    private void On_RevertHumanCanceled(object sender, EventArgs e)
    {
        Debug.Log("I AM STILL " + m_currentForm.name);
    }

    private void OnTransformPressed(object sender, EventArgs e)
    {
        TransformInto(m_selectedForm);
    }

    private void OnNextFormPressed(object sender, EventArgs e)
    {
        if(m_Transformations.Count == 0) return;
        CycleThroughTransformations();
    }

    //For resetting the game
    public void ClearTransformations()
    {
        m_Transformations.Clear();
        m_selectedForm = null;
    }

    
}
