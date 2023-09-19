using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Unity.Loading;

public class LoadScreen : MonoBehaviour
{
    [SerializeField] private GameObject m_MainLoadScreen;
    [SerializeField] private TextMeshProUGUI m_Percentage;
    [SerializeField] private Image [] m_FillImages;
    [SerializeField] private GameObject [] m_LoadScreens;
    [SerializeField] private float m_FillSpeed = 0.25f;

    private float m_CurrentFill = 0;
    private float m_targetFill = 1;
    private int m_RandomIndex;

    void Start() 
    {
        m_MainLoadScreen.SetActive(false);
    }

    void Update() 
    {
        Loading();
    }

    public void LoadScene(string scene)
    {
        StartCoroutine(loadScene_Coroutine(scene));
    }

    private IEnumerator loadScene_Coroutine(string scene)
    {
        m_MainLoadScreen.SetActive(true);

        SetLoadScreen();

        m_CurrentFill = 0;

        AsyncOperation Scene = SceneManager.LoadSceneAsync(scene);
        Scene.allowSceneActivation = false;

        while (!Scene.isDone)
        {
            // If the loading progress reaches 0.9f, set target progress to 1f
            if (Scene.progress >= 0.9f)
            {
                m_targetFill = 1f;
            }
            else
            {
                // Otherwise, update the target progress based on the scene's loading progress
                m_targetFill = Scene.progress;
            }

            // If the loading progress reaches 0.9f and fill amount is full, activate the scene
            if (Scene.progress >= 0.9f && Mathf.Approximately(m_FillImages[m_RandomIndex].fillAmount, 1f))
            {
                yield return new WaitForSeconds(1f);
                Scene.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    private void SetLoadScreen() 
    {
        m_RandomIndex = UnityEngine.Random.Range(0, m_LoadScreens.Length);

        for(int i = 0; i < m_LoadScreens.Length; i++) 
        {
            if(m_LoadScreens[i] != null) 
            {
                if(i != m_RandomIndex) 
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

    private void Loading() 
    {
        if(m_FillImages[m_RandomIndex] != null) 
        {
            m_CurrentFill = Mathf.MoveTowards(m_CurrentFill, m_targetFill, Time.deltaTime * m_FillSpeed);
                
            UpdatePercentageText(m_FillImages[m_RandomIndex]);
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
