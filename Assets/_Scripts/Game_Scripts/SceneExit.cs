using UnityEngine;

public class SceneExit : MonoBehaviour, IInteractable
{
    [SerializeField] private string m_InteractionText;
    [SerializeField] private SceneField m_TargetScene;
    [SerializeField] private SceneField m_ExitIdentifier;
    
    private bool m_IsInteracting = false;
    public bool IsInteracting => m_IsInteracting;


    public void Interact()
    {
        // I may not need to use this isInteracting thing
        if(m_IsInteracting) {return;}
        m_IsInteracting = true;

        PlayerPrefs.SetString("LastExit", m_ExitIdentifier);
        PlayerPrefs.Save();

        LoadScreen.Instance.LoadScene(m_TargetScene);

        if(LoadScreen.Instance.FinishedLoading) 
        {
            m_IsInteracting = false;
        }
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
