using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource audioSource;

    public AudioClip _loading;
    public AudioClip _meat;
    public AudioClip _build;
    public AudioClip _shop;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void LoadingSound()
    {
        audioSource.clip = _loading;
        audioSource.Play();
    }

    public void MeatCollectingSound()
    {
        audioSource.clip = _meat;
        audioSource.Play();
    }

    public void BuildModeSound()
    {
        audioSource.clip = _build;
        audioSource.Play();
    }

    public void ShopSound()
    {
        audioSource.clip = _shop;
        audioSource.Play();
    }
}
