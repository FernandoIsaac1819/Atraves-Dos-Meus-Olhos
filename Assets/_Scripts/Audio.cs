using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Audio 
{
    public string Name;
    
    [HideInInspector] public AudioSource AudioSource;
    public AudioClip AudioClip;

    [Range(0f ,1f)]
    public float Volume;
    [Range(0.1f, 3)]
    public float Pitch;

    public bool Loop;
    public bool PlayOnAwake;
}
