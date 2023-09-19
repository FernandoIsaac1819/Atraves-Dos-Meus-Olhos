using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forms : MonoBehaviour
{
    [Header("Forms")]
    [SerializeField] private GameObject m_HumanForm;
    [SerializeField] private GameObject m_CatForm;

    void SetCatForm() 
    {
        m_CatForm.SetActive(true);
    }

    void RemoveCatForm() 
    {
        m_CatForm.SetActive(false);
    }

    void SetHumanForm() 
    {
        m_HumanForm.SetActive(true);
    }

    void RemoveHumanForm() 
    {
        m_HumanForm.SetActive(false);
    }
}
