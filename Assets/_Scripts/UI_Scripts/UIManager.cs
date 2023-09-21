using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Manages the UI elements and transitions for the game, including the start menu.
/// </summary>
public class UIManager : MonoBehaviour
{
    // Singleton instance for the UIManager.
    public static UIManager Instance {get; private set;}

    private EventHandler OnGameStarted;
    
    [Header("UI Animators")]
    [SerializeField] private Animator m_StartMenuPanelAnimator;

    [Header("UI Panels")]
    [SerializeField] private GameObject m_StartMenuPanel;

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

        LoadStartMenu();

    }

    /// <summary>
    /// Loads and displays the start menu.
    /// </summary>
    private void LoadStartMenu()
    {
        m_StartMenuPanel.SetActive(true);
        m_StartMenuPanelAnimator.SetBool("StartOn", true);
    }

    /// <summary>
    /// Initiates the game start process, hiding the start menu.
    /// </summary>
    public void StartGame() 
    {
        m_StartMenuPanelAnimator.SetBool("StartOn", false);
    }
}
