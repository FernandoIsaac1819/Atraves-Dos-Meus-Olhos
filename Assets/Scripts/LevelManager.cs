using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class LevelManager: MonoBehaviour
{
    public static LevelManager Instance {get; private set;}
    private LoadScreen m_LoadScreen;
    
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

    void Start() 
    {
        m_LoadScreen = GetComponent<LoadScreen>();
    }

    public void LoadScene(string SceneName) 
    {
        var scene = SceneManager.LoadSceneAsync(SceneName);
        scene.allowSceneActivation = false;

        LoadScreen.Instance.SetLoadImage();  
    }
}
