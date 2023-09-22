using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterScene : MonoBehaviour
{
    [SerializeField] private string m_PreviousSceneName;

    void Start() 
    {
        if(PlayerPrefs.GetString("CurrentSceneName") == m_PreviousSceneName) 
        {
            Player.Instance.transform.position = transform.position;
            Player.Instance.transform.eulerAngles = transform.rotation.eulerAngles;
        }
    }

}
