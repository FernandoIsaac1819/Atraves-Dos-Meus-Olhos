using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance {get; private set;}

    private EventHandler OnGameStarted;
    
    [Header("UI Animators")]
    [SerializeField] private Animator m_StartMenuPanelAnimator;

    [Header("UI Panels")]
    [SerializeField] private GameObject m_StartMenuPanel;

    //private float m_ExitStartMenuTime = 6;
    private bool m_InStartMenu;

    //GETTERS AND SETTERS
    public bool InStartMenu {get {return m_InStartMenu;} set {m_InStartMenu = value;}}

    void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // This ensures that the instance isn't destroyed when loading a new scene.
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Ensures that there's only one instance of the script in the scene.
        }

        LoadStartMenu();
    }

    private void LoadStartMenu()
    {
        m_InStartMenu = true;
        m_StartMenuPanel.SetActive(true);
        m_StartMenuPanelAnimator.SetBool("StartOn", true);
    }

    public void StartGame() 
    {
        m_InStartMenu = false;
        m_StartMenuPanelAnimator.SetBool("StartOn", false);
        //check when the animation is done to turn it off instead of using a coroutine
        //StartCoroutine(DeactivateStartCanvas(m_ExitStartMenuTime));
    }

    IEnumerator DeactivateStartCanvas (float time) 
    {
        yield return new WaitForSeconds(time);
        m_StartMenuPanel.SetActive(false);
    }
    
}
