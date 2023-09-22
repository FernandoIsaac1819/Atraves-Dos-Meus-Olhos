using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour
{
    public static LoadScreen instance;
    [SerializeField] private CanvasGroup m_LoadScreensCanvas;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private GameObject [] m_LoadScreens;
    [SerializeField] private Image [] m_FillImages;
    [SerializeField] private TextMeshProUGUI m_Percentage;
    [SerializeField] private float m_FillSpeed;

    private int m_CurrentScreenIndex;
    private float m_TargetFill = 1;
    private float m_CurrentFill = 0;
    private bool m_CanLoad;


    void Awake() 
    {
        if(instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else if(instance != this) 
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        m_LoadScreensCanvas.alpha = 0;

        m_Animator = GetComponent<Animator>();

        for(int i = 0; i < m_LoadScreens.Length; i++) 
        {
            m_LoadScreens[i].SetActive(false);
        }
    }


    void Update()
    {
        if(m_CanLoad) 
        {
            if(m_LoadScreens[m_CurrentScreenIndex].activeSelf) 
            {
                m_CurrentFill = Mathf.MoveTowards(m_CurrentFill, m_TargetFill, Time.deltaTime * m_FillSpeed);

                UpdatePercentageText(m_LoadScreens[m_CurrentScreenIndex].transform.Find("Fill").GetComponent<Image>());
            }
        }
    }

    public void LoadScene( string sceneName) 
    {
        StartCoroutine(LoadSceneInBackground(sceneName));
    }


    private IEnumerator LoadSceneInBackground(string sceneName)
    {
        SetLoadScreen();

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
    
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone || m_CurrentFill < m_TargetFill)
        {

            if (asyncOperation.progress >= 0.9f && Mathf.Approximately(m_FillImages[m_CurrentScreenIndex].fillAmount, 1f))
            {
                m_Animator.SetTrigger("FadeOut");
            
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }


    private void SetLoadScreen() 
    {
        m_Animator.SetTrigger("FadeIn");

        ResetLoadScreen();

        m_CurrentScreenIndex += 1;
        m_CurrentScreenIndex = m_CurrentScreenIndex % m_LoadScreens.Length;

        for(int i = 0; i < m_LoadScreens.Length; i++) 
        {
            if(i == m_CurrentScreenIndex) m_LoadScreens[i].SetActive(true); 
            else m_LoadScreens[i].SetActive(false);
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

    private void CanLoadScene() 
    {
        m_CanLoad = true;
    }

    /// <summary>
    /// Resets the values of the load screen. Fill is reset, Visibility and the CanLoad bool
    /// </summary>
    void ResetLoadScreen() 
    {
        m_CanLoad = false;
        m_LoadScreensCanvas.alpha = 1;
        m_CurrentFill = 0;
    }

}
