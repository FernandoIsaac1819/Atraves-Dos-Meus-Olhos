using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;
using Unity.VisualScripting;

public class WaitScreen : MonoBehaviour
{
    public static WaitScreen Instance {get; private set;}
    public EventHandler OnLoadingFinished;

    [SerializeField] private TextMeshProUGUI m_Percentage;

    [SerializeField] private Image [] fillimages;

    [SerializeField] private GameObject [] m_LoadPanels;

    [SerializeField] private float m_FillSpeed = 0.1f;

    private float m_CurrentFill = 0;
    private float m_targetFill = 1;
    private int m_RandomIndex;

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
    }

    void Update()
    {
        
    }

    void Loading() 
    {
        for(int i = 0; i < fillimages.Length; i++) 
        {
            if(i == m_RandomIndex) 
            {
                m_CurrentFill = Mathf.MoveTowards(m_CurrentFill, m_targetFill, Time.deltaTime * m_FillSpeed);
                
                UpdatePercentageText(fillimages[i]);
            }
        }

        if(m_CurrentFill == m_targetFill) 
        {
            StartCoroutine(InvokeLoadingFinished());
        }
    }

    public IEnumerator InvokeLoadingFinished() 
    {
        yield return new WaitForSeconds(0.5f);
        OnLoadingFinished?.Invoke(this, EventArgs.Empty);
    }

    public void SetRandomLoadImage() 
    {
        m_CurrentFill = 0;

        m_RandomIndex = UnityEngine.Random.Range(0, m_LoadPanels.Length);

        for(int i = 0; i < m_LoadPanels.Length; i++) 
        {
            if(i != m_RandomIndex) 
            {
                m_LoadPanels[i].SetActive(false);
            }
            else 
            {
                m_LoadPanels[i].SetActive(true);
            }
        }
    }

    private void UpdatePercentageText(Image fill)
    {
        fill.fillAmount = m_CurrentFill;
        int percent = Mathf.RoundToInt(m_CurrentFill * 100);
        m_Percentage.text = percent + "%";
    }
}

