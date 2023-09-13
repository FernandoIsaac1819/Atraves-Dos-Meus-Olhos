using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emote : MonoBehaviour
{
    //THIS SCRIPT CONTROLS THE EMOTES OF THE CHARACTER 
    [SerializeField] private Animator m_Animator;
    [SerializeField] private PlayerAnimation m_PlayerAnimation;

    private List<int> m_DanceHashCodes = new List<int>();
    private int m_CurrentDanceIndex = 0;
    private bool m_CanChange = true;

    void Start() 
    {
        m_Animator = GetComponent<Animator>();

        // Add the hash codes to the list
        m_DanceHashCodes.Add(Animator.StringToHash("Dance1"));
        m_DanceHashCodes.Add(Animator.StringToHash("Dance2"));
        m_DanceHashCodes.Add(Animator.StringToHash("Dance3"));
        m_DanceHashCodes.Add(Animator.StringToHash("Dance4"));
    }

    void Update() 
    {
        if(HandleInputs.Instance.IsEmotePressed() && m_CanChange) 
        {
            PlayNextDance();
        }

        if(HandleInputs.Instance.IsMoving()) 
        {
            for(int i = 0; i < m_DanceHashCodes.Count; i++) 
            {
                
            }
        }
    }

    public void PlayNextDance()
    {
        foreach (int danceHash in m_DanceHashCodes)
        {
            m_Animator.ResetTrigger(danceHash);
        }

        m_Animator.SetTrigger(m_DanceHashCodes[m_CurrentDanceIndex]);

        // Increment the counter and wrap it around if it exceeds the list count
        m_CurrentDanceIndex = (m_CurrentDanceIndex + 1) % m_DanceHashCodes.Count;

        StartCoroutine(ResetCanChange());
    }

    IEnumerator ResetCanChange () 
    {
        m_CanChange = false;
        yield return new WaitForSeconds(1);
        m_CanChange = true;
    }


}
