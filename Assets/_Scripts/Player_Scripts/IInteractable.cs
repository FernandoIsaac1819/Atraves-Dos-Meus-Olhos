using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    string GetInteractionText();
    bool IsInteracted { get; }
    //string InteractionText { get; }
    void Interact();
    Transform GetTransform();
}
