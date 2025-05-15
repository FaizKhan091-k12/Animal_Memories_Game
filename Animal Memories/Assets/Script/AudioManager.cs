using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    [SerializeField] AudioSource audioSource;

    [SerializeField] private AudioClip popClip;
    [SerializeField] private AudioClip popClip2;

    private void Awake()
    {
        instance = this;
    }

    public void PlayPopClip()
    {
       audioSource.PlayOneShot(popClip);
    }

    public void PlayLevelPopClip()
    {
        audioSource.PlayOneShot(popClip2);
    }


}
