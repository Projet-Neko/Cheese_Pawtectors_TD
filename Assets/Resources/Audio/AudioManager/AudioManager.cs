using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioSource _titleScreen;
    [SerializeField] private AudioSource _mainScreen;
    private bool _titleIsPlaying = false;
    private bool _mainIsPlaying = false;
    public void StartTitleMusic()
    {
        if (!_titleIsPlaying)
        {
            _titleScreen.Play();
            _mainScreen.Stop();
            _titleIsPlaying = true;
            _mainIsPlaying = false;
        }
    }
    public void StartMainMusic()
    {
        if (!_mainIsPlaying)
        {
            _mainScreen.Play();
            _titleScreen.Stop();
            _titleIsPlaying = false;
            _mainIsPlaying = true;
        }
    }
}
