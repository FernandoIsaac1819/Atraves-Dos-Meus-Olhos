using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] float m_interactionRange;

    void Update()
    {
        Collider [] colliders = Physics.OverlapSphere(transform.position, m_interactionRange);   

        foreach(Collider collider in colliders) 
        {
            if(collider.TryGetComponent(out IInteractable interactable)) 
            {
                interactable.ShowIcon();

                if(HandleInputs.Instance.IsInteractPressed()) interactable.Interact();
            }
        }
    }
}
