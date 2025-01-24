using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractUI : MonoBehaviour
{
    public static PlayerInteractUI Instance {get; private set;}

    [SerializeField] private Image m_InteractLoadingImage;
    [SerializeField] private GameObject InteractImage;
    [SerializeField] private TextMeshProUGUI m_InteractionUGUI;
    [SerializeField] private float fillSpeed;

    private float m_CurrentFill = 0;
    private float m_TargetFill = 1;

    private bool m_IsInteractPressed;
    private bool m_CanActivateInteraction = false;
    public bool CanActivateInteraction {get {return m_CanActivateInteraction;} set {m_CanActivateInteraction = value;}}

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
        HandleInputs.Instance.OnInteractPressed += OnInteract_Pressed;
        HandleInputs.Instance.OnInteractReleased += OnInteract_Released;
    }

    private void OnInteract_Released(object sender, EventArgs e)
    {
        m_IsInteractPressed = false;
        m_CanActivateInteraction = false;
    }

    private void OnInteract_Pressed(object sender, EventArgs e)
    {
        m_IsInteractPressed = true;
        m_CurrentFill = 0;
    }

    void Update()
    {
        ShowInteractIcon();
        
        if(m_IsInteractPressed) 
        {
            LoadInteractIcon();
        }
        else 
        {
            m_InteractLoadingImage.fillAmount = 0;
        }
    }

    void LoadInteractIcon() 
    {
       m_CurrentFill = Mathf.MoveTowards(m_CurrentFill, m_TargetFill, fillSpeed * Time.unscaledDeltaTime);
       m_InteractLoadingImage.fillAmount = m_CurrentFill;

       if (Mathf.Approximately(m_CurrentFill, m_TargetFill)) 
       {
           if (!m_CanActivateInteraction) 
           {
               m_CanActivateInteraction = true; // Only update if not already true
           }

           Hide();
       }

    }

    void ShowInteractIcon() 
    {
        IInteractable interactable = PlayerInteract.Instance.GetInteractable();

        if(interactable != null) 
        {
            if(interactable.IsInteracting) {return;}
            Show(PlayerInteract.Instance.GetInteractable());
        }
        else 
        {
            Hide();
        }
    }


    public void Show(IInteractable interactable) 
    {
        InteractImage.SetActive(true);
        m_InteractionUGUI.text = interactable.GetInteractionText();
    }

    public void Hide() 
    {
        InteractImage.SetActive(false);
        m_CurrentFill = 0;
    }
}
