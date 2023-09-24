using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    bool IsInteracted { get; }
    public void ShowIcon();
    public void Interact();
}
