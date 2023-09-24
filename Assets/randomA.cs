using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomA : MonoBehaviour, IInteractable
{
    public bool m_IsInteracted = false;
    public bool IsInteracted => m_IsInteracted;

    public void Interact()
    {
        throw new System.NotImplementedException();
    }

    public void ShowIcon()
    {
        throw new System.NotImplementedException();
    }

}
