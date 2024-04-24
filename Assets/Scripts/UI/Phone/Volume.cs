using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    [SerializeField] private Slider _sliderMusic;
    [SerializeField] private Slider _sliderSound;

    static public event Action<float> OnMusicVolumeChange;
    static public event Action<float> OnSoundVolumeChange;

    public void MusicVolume()
    {
        OnMusicVolumeChange?.Invoke(_sliderMusic.value);
    }

    public void SoundVolume()
    {
        OnSoundVolumeChange?.Invoke(_sliderSound.value);
    }
}
