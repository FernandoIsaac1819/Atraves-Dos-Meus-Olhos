using System;
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
    private bool m_IsEmoting = false;

    void Start() 
    {
        m_Animator = GetComponent<Animator>();

        // Add the hash codes to the list
        m_DanceHashCodes.Add(Animator.StringToHash("Dance1"));
        m_DanceHashCodes.Add(Animator.StringToHash("Dance2"));
        m_DanceHashCodes.Add(Animator.StringToHash("Dance3"));
        m_DanceHashCodes.Add(Animator.StringToHash("Dance4"));

        HandleInputs.Instance.OnEmotePressed += OnEmote_Pressed;
        HandleInputs.Instance.OnEmoteReleased += OnEmote_Released;
    }

    private void OnEmote_Pressed(object sender, EventArgs e)
    {
        m_IsEmoting = true;
    }

    private void OnEmote_Released(object sender, EventArgs e)
    {
        m_IsEmoting = false;
    }

    void Update() 
    {
        if(m_IsEmoting && m_CanChange) 
        {
            PlayNextDance();
        }
    }


    public void PlayNextDance()
    {
        foreach (int danceHash in m_DanceHashCodes)
        {
            m_Animator.ResetTrigger(danceHash);
        }

        m_Animator.SetTrigger(m_DanceHashCodes[m_CurrentDanceIndex]);

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
