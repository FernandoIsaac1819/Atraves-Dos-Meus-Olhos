using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToScene : MonoBehaviour
{
    [SerializeField] private string m_NextSceneName;
    [SerializeField] private string m_CurrentSceneName;


    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player")) 
        {
            PlayerPrefs.SetString("CurrentSceneName", m_CurrentSceneName);
            LoadScreen.instance.LoadScene(m_NextSceneName);
        }
    }

}
