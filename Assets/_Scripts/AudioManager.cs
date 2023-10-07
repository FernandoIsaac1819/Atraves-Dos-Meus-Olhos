using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {get; private set;}
    [SerializeField] private Audio [] m_Audios;
    [SerializeField] private AudioSource m_AudioSource;

    void Awake() 
    {
        ImplementSingleton();
        InitializeAudio();
    }

    public void PlayPlayerSounds(string audioName) 
    {
        Audio AudioToPlay = Array.Find(m_Audios, m_Audios => m_Audios.Name == audioName);
        SetAudioSource(AudioToPlay);
        m_AudioSource.Play();
    }

    void SetAudioSource(Audio audioToPlay) 
    {
        m_AudioSource.clip = audioToPlay.AudioClip;
        m_AudioSource.volume = audioToPlay.Volume;
        m_AudioSource.pitch = audioToPlay.Pitch;
    }

    private void InitializeAudio() 
    {
        m_AudioSource.playOnAwake = false;

        foreach(Audio audio in m_Audios) 
        {
            m_AudioSource.clip = audio.AudioClip;
            m_AudioSource.volume = audio.Volume;
            m_AudioSource.pitch = audio.Pitch;
        }
    }

    private void ImplementSingleton() 
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
}
