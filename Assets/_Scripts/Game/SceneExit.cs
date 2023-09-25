using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneExit : MonoBehaviour, IInteractable
{
    [SerializeField] private string m_InteractionText;
    
    private bool m_IsInteracting = false;
    public bool IsInteracted => m_IsInteracting;

    [SerializeField] private string m_TargetScene;
    [SerializeField] private string m_ExitIdentifier;

    public void Interact()
    {
        if(m_IsInteracting) {return;}
        m_IsInteracting = true;

        PlayerPrefs.SetString("LastExit", m_ExitIdentifier);
        PlayerPrefs.Save();

        LoadScreen.instance.LoadScene(m_TargetScene);

        if(LoadScreen.instance.FinishedLoading) 
        {
            m_IsInteracting = false;
        }
    }

    public void ShowIcon()
    {
        Debug.Log("Go to scene " + m_TargetScene);
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public string GetInteractionText()
    {
        return m_InteractionText;
    }
}
