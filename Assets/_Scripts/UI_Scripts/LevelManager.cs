using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelManager: MonoBehaviour
{
    public delegate void LoadingProgress(float progress);
    public event LoadingProgress OnLoadingProgress;

    public static LevelManager Instance { get; private set; }

    private void Awake()
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
        
    }

    public void LoadScene(string scene)
    {
        StartCoroutine(LoadScene_Coroutine(scene));
    }

    private IEnumerator LoadScene_Coroutine(string scene)
    {
        AsyncOperation sceneLoading = SceneManager.LoadSceneAsync(scene);
        sceneLoading.allowSceneActivation = false;

        while (!sceneLoading.isDone)
        {
            float progress = Mathf.Clamp01(sceneLoading.progress / 0.9f);
            OnLoadingProgress?.Invoke(progress);

            if (sceneLoading.progress >= 0.9f)
            {
                yield return new WaitForSeconds(1f);
                sceneLoading.allowSceneActivation = true;
            }

            yield return null;
        }
    }    
}
