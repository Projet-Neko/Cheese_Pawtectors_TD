using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Mod_Audio : Module
{
    [Header("Musics")]
    [SerializeField] private AudioSource _titleScreen;
    [SerializeField] private AudioSource _mainScreen; // droit d'auteur ? 
    [SerializeField] private AudioSource _build;
    [SerializeField] private AudioSource _shop; // droit d'auteur
    [SerializeField] private AudioSource _boss;

    [Header("SoundEffects")]
    [SerializeField] private AudioClip _loading;
    [SerializeField] private AudioClip _meat;
    [SerializeField] private AudioClip _merge;
    [SerializeField] private AudioClip _albinos;
    [SerializeField] private AudioClip _button;
    [SerializeField] private AudioClip _newRoomOrCat;
    [SerializeField] private AudioClip _mouse;
    [SerializeField] private AudioClip _chase;
    [SerializeField] private AudioClip _catBox;
    [SerializeField] private AudioClip _addCat;
    [SerializeField] private AudioClip _fight;
    [SerializeField] private AudioClip _catFull;
    [SerializeField] private AudioClip _dragCoin;
    [SerializeField] private AudioClip _dropCoin;
    [SerializeField] private AudioClip _full;

    [SerializeField] private float _sliderMusic;
    [SerializeField] private float _soundVolume;

    private string _previousScene = "";
    private string _previousAdditiveScene = "";

    private bool _titleIsPlaying = false;
    private bool _mainIsPlaying = false;
    private bool _buildIsPlaying = false;
    private bool _shopIsPlaying = false;
    private bool _bossIsPlaying = false;

    private void Awake()
    {
        //Mod_Waves.BossWave += StartBossMusic;
        Mod_Waves.OnBossDefeated += StartMainMusic;
        Volume.OnMusicVolumeChange += SetMusicVolume;
        Volume.OnSoundVolumeChange += SetSoundVolume;
    }

    private void OnDestroy()
    {
        //Mod_Waves.BossWave -= StartBossMusic;
        Mod_Waves.OnBossDefeated -= StartMainMusic;
        Volume.OnMusicVolumeChange -= SetMusicVolume;
        Volume.OnSoundVolumeChange -= SetSoundVolume;

    }

    public override void Init(GameManager gm)
    {
        base.Init(gm);
        InitComplete();
    }

    public void SetMusicVolume(float volume)
    {
        //float volume = _sliderMusic.value ;
        _titleScreen.volume = volume;
        _mainScreen.volume = volume;
        _build.volume = volume;
        _shop.volume = volume;
        _boss.volume = volume;
    }

    public void SetSoundVolume(float volume)
    {
        _soundVolume = volume;
    }


    public void StartMusic(string sceneName)
    {

        if (_previousAdditiveScene == sceneName)
        {
            sceneName = _previousScene;
            _previousAdditiveScene = "";
        }
        switch (sceneName)
        {
            case "MainScreen":
                StopMusic();
                _mainScreen.Play();
                _previousScene = sceneName;
                break;

            case "Build":
                StopMusic();
                _build.Play();
                _previousScene = sceneName;
                break;

            case "Catalog":
                StopMusic();
                _shop.Play();
                _previousAdditiveScene = sceneName;
                break;

            default:
                break;
        }
    }

    private void StartBossMusic()
    {
        StopMusic();
        _boss.Play();
    }

    private void StartMainMusic()
    {
        StopMusic();
        _boss.Play();
    }

    private void StopMusic()
    {
        _titleScreen.Stop();
        _mainScreen.Stop();
        _build.Stop();
        _shop.Stop();
        _boss.Stop();
    }

    public void PlayLoadingSound()
        => _titleScreen.PlayOneShot(_loading, _soundVolume);

    public void PlayMeatSound()
        => _titleScreen.PlayOneShot(_meat, _soundVolume);

    public void PlayMergeSound()
        => _titleScreen.PlayOneShot(_merge, _soundVolume);

    public void PlayAlbinosSound()
        => _titleScreen.PlayOneShot(_albinos, _soundVolume);

    public void PlayButtonSound()
        => _titleScreen.PlayOneShot(_button, _soundVolume);

    public void PlayNewRoomOrCatSound()
        => _titleScreen.PlayOneShot(_newRoomOrCat, _soundVolume);

    public void PlayMouseSound()
        => _titleScreen.PlayOneShot(_mouse, _soundVolume);

    public void PlayChaseSound()
        => _titleScreen.PlayOneShot(_chase, _soundVolume);

    public void PlayCatBoxSound()
        => _titleScreen.PlayOneShot(_catBox, _soundVolume);

    public void PlayAddCatSound()
        => _titleScreen.PlayOneShot(_addCat, _soundVolume);

    public void PlayFightSound()
        => _titleScreen.PlayOneShot(_fight, _soundVolume);

    public void PlayCatFullSound()
        => _titleScreen.PlayOneShot(_catFull, _soundVolume);

    public void PlayDragCoinSound()
        => _titleScreen.PlayOneShot(_dragCoin, _soundVolume);

    public void PlayDropCoinSound()
        => _titleScreen.PlayOneShot(_dropCoin, _soundVolume);

    public void PlayFullSound()
        => _titleScreen.PlayOneShot(_full, _soundVolume);



}
