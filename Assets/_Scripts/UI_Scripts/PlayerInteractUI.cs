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

    [SerializeField] private Image m_InteractLoadingImage;
    [SerializeField] private GameObject InteractImage;
    [SerializeField] private TextMeshProUGUI m_InteractionUGUI;
    [SerializeField] private float fillSpeed;
    private float m_CurrentFill = 0;
    private float m_TargetFill = 1;
    private bool m_IsInteractIconloading;
    public EventHandler OnInteractLoaded;
    public static PlayerInteractUI Instance {get; private set;}


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
        m_IsInteractIconloading = false;
    }

    private void OnInteract_Pressed(object sender, EventArgs e)
    {
        m_IsInteractIconloading = true;
        m_CurrentFill = 0;
    }

    void Update()
    {
        ShowInteractIcon();

        if(m_IsInteractIconloading) 
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

        if(Mathf.Approximately(m_CurrentFill, m_TargetFill)) 
        {
            OnInteractLoaded?.Invoke(this, EventArgs.Empty);
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
            m_CurrentFill = 0;
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
    }
}
