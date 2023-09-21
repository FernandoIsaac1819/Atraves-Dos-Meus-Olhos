using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

/// <summary>
/// Handles the screen loader
/// </summary>
public class WaitScreen : MonoBehaviour
{
    
    public static WaitScreen Instance {get; private set;}

    [SerializeField] private Animator m_Animator;
    [SerializeField] private GameObject m_MainLoadScreen;
    [SerializeField] private TextMeshProUGUI m_Percentage;
    [SerializeField] private Image [] m_FillImages;
    [SerializeField] private GameObject [] m_LoadScreens;
    [SerializeField] private float m_FillSpeed = 0.25f;
    
    private float m_CurrentFill = 0;
    private float m_targetFill = 1;
    private int m_CurrentScreenIndex = 0;

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
        for(int i = 0; i < m_LoadScreens.Length; i++) 
        {
            if(m_LoadScreens[i] != null) 
            {
                m_LoadScreens[i].SetActive(false);
            }
        }
    }

    void Update() 
    {
        Loading();
    }

    public void LoadScene(string scene)
    {
        StartCoroutine(loadScene_Coroutine(scene));
    }

    private void ActivateLoadScreen() 
    {
        for(int i = 0; i < m_LoadScreens.Length; i++) 
        {
            if(m_LoadScreens[i] != null) 
            {
                if(i != m_CurrentScreenIndex) 
                {
                    m_LoadScreens[i].SetActive(false);
                }
                else 
                {
                    m_LoadScreens[i].SetActive(true);
                }
            }
        }
    }

    private IEnumerator loadScene_Coroutine(string scene)
    {
        m_Animator.SetTrigger("FadeIn");

        m_CurrentFill = 0;

        m_CurrentScreenIndex += 1;
        m_CurrentScreenIndex = m_CurrentScreenIndex % m_LoadScreens.Length; 

        ActivateLoadScreen();

        AsyncOperation Scene = SceneManager.LoadSceneAsync(scene);
        Scene.allowSceneActivation = false;

        while (!Scene.isDone)
        {
            if (Scene.progress >= 0.9f)
            {
                m_targetFill = 1f;
            }
            else
            {
                m_targetFill = Scene.progress;
            }

            if (Scene.progress >= 0.9f && Mathf.Approximately(m_FillImages[m_CurrentScreenIndex].fillAmount, 1f))
            {
                Scene.allowSceneActivation = true;
                m_Animator.SetTrigger("FadeOut");
            }

            yield return null;
        }
    }

    private void Loading() 
    {
        if(m_FillImages[m_CurrentScreenIndex] != null) 
        {
            m_CurrentFill = Mathf.MoveTowards(m_CurrentFill, m_targetFill, Time.deltaTime * m_FillSpeed);
                
            UpdatePercentageText(m_FillImages[m_CurrentScreenIndex]);
        }
    }

    private void UpdatePercentageText(Image image)
    {
        image.fillAmount = m_CurrentFill;
        int percent = Mathf.RoundToInt(m_CurrentFill * 100);
        if(m_Percentage != null) 
        {
            m_Percentage.text = percent + "%";
        }
    }
}