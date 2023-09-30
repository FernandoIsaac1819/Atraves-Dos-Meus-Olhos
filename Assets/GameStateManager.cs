using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameStateManager : MonoBehaviour
{
    public static void PauseGame() 
    {
        Time.timeScale = 0;
        HandleInputs.Instance.PlayerInputActions.Player.Jump.Disable(); 
    }

    public static void ResumeGame() 
    {
        HandleInputs.Instance.PlayerInputActions.Player.Jump.Enable();
        Time.timeScale = 1f;
    }

    public void QuitGame() 
    {
        Application.Quit();
    }
}
