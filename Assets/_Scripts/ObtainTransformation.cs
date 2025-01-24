using System.Collections;
using UnityEngine;

public class ObtainTransformation : MonoBehaviour, IInteractable
{
    [SerializeField] private TransformationBase_SO m_Transformation;
    
    private bool m_IsInteracting = false;
    public bool IsInteracting => m_IsInteracting;


    public void Interact()
    {
        if(m_IsInteracting) {return;}
        m_IsInteracting = true;
        
        StartCoroutine(AddTransformation(m_Transformation));
    }

    private IEnumerator AddTransformation(TransformationBase_SO form) 
    {
        TransformationManager.Instance.AddNewTransformation(form);
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }

    public string GetInteractionText()
    {
        return m_Transformation.name;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
