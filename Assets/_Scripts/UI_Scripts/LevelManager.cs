using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class LevelManager: MonoBehaviour
{
    [SerializeField] private GameObject m_MainLoadScreen;
    [SerializeField] private TextMeshProUGUI m_Percentage;
    [SerializeField] private Image [] m_Images;
    [SerializeField] private GameObject [] m_LoadScreens;
    [SerializeField] private float m_FillSpeed = 0.1f;
    public Image loadImage;
    private float targetProgress = 0f;
    private float m_CurrentFill = 0;
    private float m_targetFill = 1;
    private int m_RandomIndex;

    void Start() 
    {
        m_MainLoadScreen.SetActive(false);
    }

    void Update()
    {
        loadImage.fillAmount = Mathf.MoveTowards(loadImage.fillAmount, targetProgress, Time.deltaTime * m_FillSpeed);
    }

    public void LoadScene(string scene)
    {
        StartCoroutine(loadScene_Coroutine(scene));
    }

    public IEnumerator loadScene_Coroutine(string scene)
    {
        m_MainLoadScreen.SetActive(true);
        loadImage.fillAmount = 0;

        AsyncOperation Scene = SceneManager.LoadSceneAsync(scene);
        Scene.allowSceneActivation = false;

        while (!Scene.isDone)
        {
            // If the loading progress reaches 0.9f, set target progress to 1f
            if (Scene.progress >= 0.9f)
            {
                targetProgress = 1f;
            }
            else
            {
                // Otherwise, update the target progress based on the scene's loading progress
                targetProgress = Scene.progress;
            }

            // If the loading progress reaches 0.9f and fill amount is full, activate the scene
            if (Scene.progress >= 0.9f && Mathf.Approximately(loadImage.fillAmount, 1f))
            {
                yield return new WaitForSeconds(1f);
                Scene.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
