using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the visuals of the Menu. Receives events from the UI manager and reacts to them herer
/// </summary>
public class Menu : MonoBehaviour
{
    public static Menu Instance {get; private set;}

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


    
}
