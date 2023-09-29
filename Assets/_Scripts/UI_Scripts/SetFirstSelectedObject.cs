using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SetFirstSelectedObject : MonoBehaviour
{
    [SerializeField] private EventSystem m_EventSystem;
    [SerializeField] private GameObject m_StartMenuButton;


    void Update() 
    {
        if(m_StartMenuButton != null) 
        {
            SetSelectedObject(m_StartMenuButton);
        }
    }


    private void SetSelectedObject(GameObject selectedObject) 
    {
        m_EventSystem.firstSelectedGameObject = selectedObject;
    }
}
