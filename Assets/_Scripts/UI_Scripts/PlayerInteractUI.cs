using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject InteractImage;
    [SerializeField] private TextMeshProUGUI m_InteractionUGUI;

    public static PlayerInteractUI Instance {get; private set;}


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

    
    void Update()
    {
        if(PlayerInteract.Instance.GetInteractable() != null) 
        {
            Show(PlayerInteract.Instance.GetInteractable());
        }
        else 
        {
            Hide();
        }
    }

    void Show(IInteractable interactable) 
    {
        InteractImage.SetActive(true);
        m_InteractionUGUI.text = interactable.GetInteractionText();
    }

    void Hide() 
    {
        InteractImage.SetActive(false);
    }
}
