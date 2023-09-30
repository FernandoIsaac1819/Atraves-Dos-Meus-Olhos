using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.UI;

public class FastTravelManager : MonoBehaviour
{
    public static FastTravelManager Instance { get; private set; }

    [SerializeField] private List<string> m_DiscoveredLocations = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start() 
    {
        DiscoverLocation(SceneManager.GetActiveScene().name);

        SceneManager.sceneLoaded += FindFirstLocation;
    }

    private void FindFirstLocation(Scene arg0, LoadSceneMode arg1)
    {
        DiscoverLocation(SceneManager.GetActiveScene().name);
    }

    public void DiscoverLocation(string locationName)
    {
        if (!m_DiscoveredLocations.Contains(locationName))
        {
            m_DiscoveredLocations.Add(locationName);
        }
    }

    public bool CanFastTravel(string locationName)
    {
        return m_DiscoveredLocations.Contains(locationName);
    }

    public void FastTravel(string locationName)  
    {
        if (CanFastTravel(locationName))  
        {
            if(SceneManager.GetActiveScene().name == locationName) 
            {
                Debug.LogWarning("Already there");
            } 
            else 
            {
                LoadScreen.Instance.LoadScene(locationName);
                UIManager.Instance.DeactivateMenu();
            }
            
        }  
        else  
        {
            Debug.LogWarning("Undiscovered!"); 
        }
    }

}
