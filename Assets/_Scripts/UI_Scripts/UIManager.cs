using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    // Singleton instance for the UIManager.
    public static UIManager Instance {get; private set;}

    [Header("Main menu panels")]
    [SerializeField] private GameObject m_MenuPanel;
    [SerializeField] private GameObject m_ControlsPanel;
    [SerializeField] private GameObject m_SettingsPanel;
    [SerializeField] private GameObject m_MapPanel;
    
    [Header("First button selection")]
    [SerializeField] private GameObject m_MenuFirstSelection;
    [SerializeField] private GameObject m_SettingsFirstSelection;
    [SerializeField] private GameObject m_ControlsFirstSelection;
    [SerializeField] private GameObject m_MapFirstSelection;

    bool m_IsMenuPanelActive = false;
    bool m_IsControlPanelActive = false;
    bool m_IsMapPanelActive = false;
    bool m_IsSettingsPanelActive = false;

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
        InitializePanels();
        HandleInputs.Instance.OnMenuPressed += OnMenu_Pressed;
    }

    void Update() 
    {
        ControlPanelActivations();
    }

    public void SetFirstSelectionButton(GameObject firstSelection) 
    {
        EventSystem.current.SetSelectedGameObject(firstSelection);
    }

    void InitializePanels() 
    {
        m_IsMenuPanelActive = false;
        m_IsMapPanelActive = false;
        m_IsControlPanelActive = false;
        m_IsSettingsPanelActive = false;
    }

    #region MenuController
    void ControlPanelActivations() 
    {
        m_SettingsPanel.SetActive(m_IsSettingsPanelActive);
        m_MenuPanel.SetActive(m_IsMenuPanelActive);
        m_MapPanel.SetActive(m_IsMapPanelActive);
        m_ControlsPanel.SetActive(m_IsControlPanelActive);
    }

    private void OnMenu_Pressed(object sender, EventArgs e)
    {
        ToggleMenu();
    }

    public void ToggleMenu() 
    {
        m_IsMenuPanelActive = !m_IsMenuPanelActive;
        m_IsMapPanelActive = false;
        m_IsControlPanelActive = false;
        m_IsSettingsPanelActive = false;

        if(m_IsMenuPanelActive) 
        {
            SetFirstSelectionButton(m_MenuFirstSelection);
            GameStateManager.PauseGame();
        }
        else 
        {
            SetFirstSelectionButton(null);
            GameStateManager.ResumeGame();
        }
    }

    public void DeactivateMenu() 
    {
        m_IsMenuPanelActive = false;
        m_IsMapPanelActive = false;
        m_IsControlPanelActive = false;
        m_IsSettingsPanelActive = false;

        GameStateManager.ResumeGame();
    }

    public void ReturnToMainMenu() 
    {
        m_IsMenuPanelActive = true;
        m_IsMapPanelActive = false;
        m_IsControlPanelActive = false;
        m_IsSettingsPanelActive = false;

        SetFirstSelectionButton(m_MenuFirstSelection);
    }

    public void ActivateControlsMenu() 
    {
        SetFirstSelectionButton(m_ControlsFirstSelection);
        m_IsMenuPanelActive = false;
        m_IsMapPanelActive = false;
        m_IsControlPanelActive = true;
        m_IsSettingsPanelActive = false;
    }

    public void ActivateSettingsMenu() 
    {
        SetFirstSelectionButton(m_SettingsFirstSelection);
        m_IsMenuPanelActive = false;
        m_IsMapPanelActive = false;
        m_IsControlPanelActive = false;
        m_IsSettingsPanelActive = true;
    }

    public void ActivateMapMenu() 
    {
        SetFirstSelectionButton(m_MapFirstSelection);
        m_IsMenuPanelActive = false;
        m_IsMapPanelActive = true;
        m_IsControlPanelActive = false;
        m_IsSettingsPanelActive = false;
    }
    #endregion

}
