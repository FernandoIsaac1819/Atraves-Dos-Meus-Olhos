using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class Map : MonoBehaviour
{
    public static Map Instance {get; private set;}

    [Header("UI Panels")]
    [SerializeField] private GameObject m_MapUI;
    

    [Header("First Selected Options")]
    [SerializeField] private GameObject m_MapFirst;

    private bool m_MapActive;
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
        m_MapUI.SetActive(false);
        
        HandleInputs.Instance.OnMapPressed += OnMap_Pressed;
        SceneManager.sceneLoaded += DeactivateActivePanels;
    }

    void ToggleMap() 
    {
        m_MapActive = !m_MapActive;
        m_MapUI.SetActive(m_MapActive);

        if(m_MapActive) 
        {
            UIManager.PauseGame(m_MapFirst);  
        } 
        else    
        {
            UIManager.ResumeGame();
        }
    }

    private void OnMap_Pressed(object sender, EventArgs e)
    {
        ToggleMap();
    }

    private void DeactivateActivePanels(Scene arg0, LoadSceneMode arg1)
    {
        m_MapUI.SetActive(false);
    }

    
}
