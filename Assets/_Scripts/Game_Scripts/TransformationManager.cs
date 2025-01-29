using System;
using System.Collections.Generic;
using UnityEngine;

public class TransformationManager : MonoBehaviour
{
    public bool debug = true;
    private Animator animator;
    public static TransformationManager Instance;

    [SerializeField] private List<TransformationBase_SO> m_forms = new List<TransformationBase_SO>(); // List of available forms
    private TransformationBase_SO m_currentForm; // Currently active form
    private TransformationBase_SO m_selectedForm; // Form selected for the next transformation
    private Dictionary<TransformationBase_SO, GameObject> m_FormPrefabs = new Dictionary<TransformationBase_SO, GameObject>(); // Maps forms to their instantiated prefabs
    private GameObject m_CurrentFormPrefab; // Currently active prefab

    public TransformationBase_SO currentForm {get {return m_currentForm;} set {m_currentForm = value;}}

    private int currentIndex;

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
        animator = GetComponent<Animator>();

        // Instantiate all form prefabs as children of the Player GameObject
        foreach (var form in m_forms)
        {
            GameObject formPrefab = Instantiate(form.prefab, PlayerMovement.Instance.transform.position, Quaternion.identity, PlayerMovement.Instance.transform);
            formPrefab.SetActive(false); // Deactivate all initially
            m_FormPrefabs.Add(form, formPrefab);
        }

        // Set the initial form
        m_currentForm = m_forms[0];
        m_CurrentFormPrefab = m_FormPrefabs[m_currentForm];
        m_CurrentFormPrefab.SetActive(true);
        m_selectedForm = m_currentForm;

        HandleInputs.Instance.OnTransform_Pressed += onTransform_Pressed;
        HandleInputs.Instance.OnNextFormPressed += onNextForm_Pressed;
    }

    public void ApplyTransformation()
    {
        if (m_selectedForm == null) return;
 
        // Deactivate the current form's prefab
        if (m_CurrentFormPrefab != null)
        {
            m_CurrentFormPrefab.SetActive(false);
        }

        // Update the current form and activate the new form's prefab
        m_currentForm = m_selectedForm;
        m_CurrentFormPrefab = m_FormPrefabs[m_currentForm];
        m_CurrentFormPrefab.SetActive(true);

        // Update player movement parameters
        PlayerMovement.Instance.UpdateFormParameters(m_currentForm);
    }

    public void AddNewTransformation(TransformationBase_SO newForm)
    {
        if (!m_forms.Contains(newForm))
        {
            m_forms.Add(newForm);

            // Instantiate the new form's prefab and make it a child of the Player GameObject
            GameObject formPrefab = Instantiate(newForm.prefab, PlayerMovement.Instance.transform.position, PlayerMovement.Instance.transform.rotation, PlayerMovement.Instance.transform);
            formPrefab.SetActive(false); // Deactivate initially
            m_FormPrefabs.Add(newForm, formPrefab);
        }
    }

    public TransformationBase_SO CycleNextForm()
    {
        currentIndex = (currentIndex + 1) % m_forms.Count;
        m_selectedForm = m_forms[currentIndex];
        Debug.Log(m_selectedForm.name);
        return m_selectedForm;
    }

    private void onNextForm_Pressed(object sender, EventArgs e)
    {
        if(m_forms.Count == 0) return;
        CycleNextForm();
        Debug.Log(m_selectedForm);
    }

    private void onTransform_Pressed(object sender, EventArgs e)
    {
        if(m_selectedForm == m_currentForm) return;
        animator.SetTrigger(AnimationHashCodes.Instance.TransformInto);
    }
}
