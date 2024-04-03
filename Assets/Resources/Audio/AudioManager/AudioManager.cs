using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioSource _titleScreen;
    [SerializeField] private AudioSource _MainScreen;
    private bool _titleIsPlaying = false;
    private bool _mainIsPlaying = false;
}
