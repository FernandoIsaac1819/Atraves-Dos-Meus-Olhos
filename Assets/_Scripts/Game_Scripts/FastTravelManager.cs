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
        StartCoroutine(FastTravel_Coroutine(locationName));
    }

    public IEnumerator FastTravel_Coroutine(string locationName)  
    {
        if (CanFastTravel(locationName))  
        {
            if(SceneManager.GetActiveScene().name == locationName) 
            {
                Debug.LogWarning("Already there");
                //Send event to the Map script to handle the visuals of the map area 
                //area becomes red when it is clicked and it's not available
            } 
            else 
            {
                LoadScreen.Instance.LoadScene(locationName);
                UIManager.Instance.DeactivateMenu();

                yield return new WaitForSeconds(1);
                HandleInputs.Instance.PlayerInputActions.Player.Jump.Enable();
            }
            
        }  
        else  
        {
            Debug.LogWarning("Undiscovered!");
            //Send event to the Map script to handle the visuals of the map area 
            //area becomes red when it is clicked and it's not available
        }
    }

}
