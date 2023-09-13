using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;

public class LoadScreen : MonoBehaviour
{
    public static LoadScreen Instance {get; private set;}

    [SerializeField] private GameObject m_LoadScreen;
    [SerializeField] private TextMeshProUGUI m_Percentage;
    [SerializeField] private Image [] fillimages;
    [SerializeField] private GameObject [] m_LoadPanels;
    [SerializeField] private float m_FillSpeed = 0.1f;

    private AsyncOperation m_Scene;

    private float m_CurrentFill = 0;
    private float m_targetFill = 1;
    private int m_RandomIndex;

    private bool isSceneReadyToActivate = false;

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
        if(m_LoadScreen != null) {m_LoadScreen.SetActive(false);}
    }

    public void SetLoadImage() 
    {
        if(m_LoadScreen != null) {m_LoadScreen.SetActive(true);}
        
        m_CurrentFill = 0;

        m_RandomIndex = UnityEngine.Random.Range(0, m_LoadPanels.Length);

        for(int i = 0; i < m_LoadPanels.Length; i++) 
        {
            if(m_LoadPanels[i] != null) 
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
    }

    public void LoadScene(string SceneName) 
    {
        m_Scene = SceneManager.LoadSceneAsync(SceneName);
        m_Scene.allowSceneActivation = false;

        SetLoadImage();
        StartCoroutine(Load());
    }

    IEnumerator Load() 
    {
        while (m_CurrentFill < m_targetFill)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1);
        m_Scene.allowSceneActivation = true;

    }
    
    void Update() 
    {
        for(int i = 0; i < fillimages.Length; i++) 
        {
            if(fillimages[i] != null) 
            {
                if(i == m_RandomIndex) 
                {
                    m_CurrentFill = Mathf.MoveTowards(m_CurrentFill, m_targetFill, Time.deltaTime * m_FillSpeed);
                
                    UpdatePercentageText(fillimages[i]);
                }
            }
        }  

    }

    void Loading() 
    {
        
    }

    /*
    void FadeOut() 
    {
        m_CanvasGroup.alpha = Mathf.MoveTowards(m_CanvasGroup.alpha, );
    }*/

    private void UpdatePercentageText(Image fill)
    {
        fill.fillAmount = m_CurrentFill;
        int percent = Mathf.RoundToInt(m_CurrentFill * 100);
        if(m_Percentage != null) 
        {
            m_Percentage.text = percent + "%";
        }
    }

}
