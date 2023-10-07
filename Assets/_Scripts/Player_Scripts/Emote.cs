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

    [SerializeField] private List<string> m_CatSounds = new List<string>();
    private List<int> m_HumanEmoteAnimations = new List<int>();

    private int m_CatEmoteIndex = 0;
    private int m_HumanEmoteIndex = 0;

    private bool m_CanChange = true;
    private bool m_IsEmoting = false;

    void Start() 
    {
        m_Animator = GetComponent<Animator>();

        //Add the hash codes to the huma list
        m_HumanEmoteAnimations.Add(Animator.StringToHash("Dance1"));
        m_HumanEmoteAnimations.Add(Animator.StringToHash("Dance2"));
        m_HumanEmoteAnimations.Add(Animator.StringToHash("Dance3"));
        m_HumanEmoteAnimations.Add(Animator.StringToHash("Dance4"));

        HandleInputs.Instance.OnEmotePressed += OnEmote_Pressed;
        HandleInputs.Instance.OnEmoteReleased += OnEmote_Released;
    }

    void Update() 
    {
        if(Transformation.Instance.IsHuman && m_IsEmoting && m_CanChange) 
        {
            PlayNextHumanEmote();
        }

        if(!Transformation.Instance.IsHuman && m_IsEmoting && m_CanChange) 
        {
            PlayNextCatEmote();
        }
    }

    public void PlayNextHumanEmote()
    {
        foreach (int emote in m_HumanEmoteAnimations)
        {
            m_Animator.ResetTrigger(emote);
        }

        m_Animator.SetTrigger(m_HumanEmoteAnimations[m_HumanEmoteIndex]);
        //AudioManager.Instance.PlayAudio(m_HumanSounds[m_humanEmoteIndex].Name)

        m_HumanEmoteIndex = (m_HumanEmoteIndex + 1) % m_HumanEmoteAnimations.Count;

        StartCoroutine(ResetCanEmote());
    }

    private void PlayNextCatEmote() 
    {
        /*
        foreach(int emote in m_CatEmoteAnimations) 
        {
            m_Animator.ResetTrigger(emote);
        }
        //m_Animator.SetTrigger(m_CatEmoteAnimations[catEmoteIndex])*/

        AudioManager.Instance.PlayPlayerSounds(m_CatSounds[m_CatEmoteIndex]);

        m_CatEmoteIndex = (m_CatEmoteIndex + 1) % m_CatSounds.Count;

        StartCoroutine(ResetCanEmote());
    }

    private void OnEmote_Pressed(object sender, EventArgs e)
    {
        m_IsEmoting = true;
    }

    private void OnEmote_Released(object sender, EventArgs e)
    {
        m_IsEmoting = false;
    }
    
    IEnumerator ResetCanEmote () 
    {
        m_CanChange = false;
        yield return new WaitForSeconds(1);
        m_CanChange = true;
    }

}
