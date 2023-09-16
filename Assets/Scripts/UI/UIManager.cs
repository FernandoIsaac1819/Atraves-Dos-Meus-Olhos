using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private float m_ExitStartMenuTime = 6;
    
    [Header("UI Animators")]
    //UI ANIMATORS
    //[SerializeField] private Animator m_MessagePanelAnimator;
    [SerializeField] private Animator m_StartMenuPanelAnimator;
    [SerializeField] private Animator m_GlowAnimator;

    [Header("UI Panels")]
    [SerializeField] private GameObject m_LoadScreenPanel;
    [SerializeField] private GameObject m_StartMenuPanel;

    void Awake() 
    {
        LoadStartMenu();
    } 

    void Start()
    {
        m_LoadScreenPanel.SetActive(false);
    }

    private void LoadStartMenu()
    {
        m_StartMenuPanel.SetActive(true);
        m_StartMenuPanelAnimator.SetBool("StartOn", true);
        m_GlowAnimator.SetTrigger("MiddleGlow");
    }

    public void StartGame() 
    {
        m_StartMenuPanelAnimator.SetBool("StartOn", false);
        StartCoroutine(DeactivateStartCanvas(m_ExitStartMenuTime));
    }

    IEnumerator DeactivateStartCanvas (float time) 
    {
        yield return new WaitForSeconds(time);
        m_StartMenuPanel.SetActive(false);
    }
    
}
