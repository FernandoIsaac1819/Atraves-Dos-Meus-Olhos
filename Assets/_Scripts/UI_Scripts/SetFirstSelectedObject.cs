using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Sets the first selected object for the EventSystem, primarily used for UI navigation.
/// </summary>
public class SetFirstSelectedObject : MonoBehaviour
{
    [SerializeField] private EventSystem m_EventSystem;
    [SerializeField] private GameObject m_StartMenuButton;

    /// <summary>
    /// Updates the selected object based on the current UI state.
    /// </summary>
    void Update() 
    {
        if(m_StartMenuButton != null) 
        {
            SetSelectedObject(m_StartMenuButton);
        }
    }

    /// <summary>
    /// Sets the provided object as the first selected object in the EventSystem.
    /// </summary>
    /// <param name="selectedObject">The game object to be set as first selected.</param>
    private void SetSelectedObject(GameObject selectedObject) 
    {
        m_EventSystem.firstSelectedGameObject = selectedObject;
    }
}
