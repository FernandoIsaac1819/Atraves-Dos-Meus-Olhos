using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script control the emote functionality. 
/// </summary>
public class Emote : MonoBehaviour
{

    private Animator m_Animator;
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
        /*
        if(HandleInputs.Instance.IsMoving()) 
        {
            for(int i = 0; i < m_DanceHashCodes.Count; i++) 
            {
                
            }
        }*/
    }

    /// <summary>
    /// Triggers the dance animations animations. Plays the next animation with every click and resets the trigger of the previous animation.
    /// </summary>
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

    /// <summary>
    /// Resets the bool allowing the player to start dancing
    /// </summary>
    /// <returns></returns>
    IEnumerator ResetCanChange () 
    {
        m_CanChange = false;
        yield return new WaitForSeconds(1);
        m_CanChange = true;
    }


}
