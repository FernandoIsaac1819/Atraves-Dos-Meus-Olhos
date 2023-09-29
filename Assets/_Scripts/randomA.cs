using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class randomA : MonoBehaviour, IInteractable
{
    
    [SerializeField] private string m_InteractionText;
    private bool m_IsInteracting = false;
    public bool IsInteracting => m_IsInteracting;

    public void Interact()
    {
        Debug.Log("Interact with random");
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
