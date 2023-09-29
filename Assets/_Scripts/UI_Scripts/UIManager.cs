using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Singleton instance for the UIManager.
    public static UIManager Instance {get; private set;}

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

    public static void PauseGame(GameObject firstSelection) 
    {
        Time.timeScale = 0;
        EventSystem.current.SetSelectedGameObject(firstSelection);
        HandleInputs.Instance.PlayerInputActions.Player.Jump.Disable(); 
    }

    public static void ResumeGame() 
    {
        HandleInputs.Instance.PlayerInputActions.Player.Jump.Enable();
        Time.timeScale = 1f;
    }

}
