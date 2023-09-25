using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public static PlayerInteract Instance {get; private set;}

    [SerializeField] private float m_InteractionRange;
    private bool m_InteractPressed;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }


    void Start() 
    {
        HandleInputs.Instance.OnInteractPressed += OnInteract_Pressed;
        HandleInputs.Instance.OnInteractReleased += OnInteract_Released;
    }


    void Update()
    {
        if(m_InteractPressed) 
        {
            IInteractable interactable = GetInteractable();

            if(interactable != null) 
            {
                interactable.Interact();
            }
        }
  
    }


    public IInteractable GetInteractable () 
    {
        Collider [] colliders = Physics.OverlapSphere(transform.position, m_InteractionRange);   

        List<IInteractable> InteractableList = new List<IInteractable>();

        foreach(Collider collider in colliders) 
        {
            if(collider.TryGetComponent(out IInteractable interactable)) 
            {
                InteractableList.Add(interactable);
            }
        }

        IInteractable closestInteratable = null;

        foreach(IInteractable interactable in InteractableList) 
        {
            if(closestInteratable == null) 
            {
                closestInteratable = interactable;
            }
            else if(Vector3.Distance(transform.position, interactable.GetTransform().position) < 
            Vector3.Distance(transform.position, closestInteratable.GetTransform().position)) 
            {
                closestInteratable = interactable;
            }
            
        }

        return closestInteratable;
    } 


    private void OnInteract_Released(object sender, EventArgs e)
    {
        m_InteractPressed = false;
    }


    private void OnInteract_Pressed(object sender, EventArgs e)
    {
        m_InteractPressed = true;
    }
}
