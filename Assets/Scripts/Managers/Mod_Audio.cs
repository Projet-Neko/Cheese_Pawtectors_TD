using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Mod_Audio : Module
{
    [Header("AudioSource")]
    [SerializeField] private AudioSource _music;
    [SerializeField] private AudioSource _sound;

    [Header("Sound")]
    [SerializeField] private AudioClip _mainScreenMusic;
    [SerializeField] private AudioClip _shopMusic;
    [SerializeField] private AudioClip _buildMusic;
    [SerializeField] private AudioClip _bossMusic;
    [SerializeField] private AudioClip _loading;
    [SerializeField] private AudioClip _treat;
    [SerializeField] private AudioClip _merge;
    [SerializeField] private AudioClip _blackMouse;
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

    [SerializeField] private float _sliderMusic = 1;
    [SerializeField] private float _soundVolume = 1;

    private string _previousScene = "";
    private string _previousAdditiveScene = "";


    private void Awake()
    {
        Mod_Waves.BossWave += StartBossMusic;
        Mod_Waves.OnBossDefeated += StartMainMusic;
        Volume.OnMusicVolumeChange += SetMusicVolume;
        Volume.OnSoundVolumeChange += SetSoundVolume;
        Mod_Economy.treatWin += PlayTreatSound;
        Merge.OnCatMerge += PlayMergeSound;
        Mod_Waves.AlbinoSpawned += PlayBlackMouseSound;
        Button.OnButtonClicked += PlayButtonSound;
    }

    private void OnDestroy()
    {
        Mod_Waves.BossWave -= StartBossMusic;
        Mod_Waves.OnBossDefeated -= StartMainMusic;
        Volume.OnMusicVolumeChange -= SetMusicVolume;
        Volume.OnSoundVolumeChange -= SetSoundVolume;
        Mod_Economy.treatWin -= PlayTreatSound;
        Merge.OnCatMerge -= PlayMergeSound;
        Mod_Waves.AlbinoSpawned -= PlayBlackMouseSound;
        Button.OnButtonClicked -= PlayButtonSound;
    }

    public override void Init(GameManager gm)
    {
        base.Init(gm);
        InitComplete();
    }

    public void SetMusicVolume(float volume)
    => _music.volume = volume;

    public void SetSoundVolume(float volume)
    => _sound.volume = volume;



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
                //StopMusic();
                _music.clip = _mainScreenMusic;
                _previousScene = sceneName;
                _music.Play();
                break;

            case "Build":
                //StopMusic();
                _music.clip = _buildMusic;
                _previousScene = sceneName;
                _music.Play();
                break;

            case "Catalog":
                //StopMusic();
                _music.clip = _shopMusic;
                _previousAdditiveScene = sceneName;
                _music.Play();
                break;

            default:
                break;
        }
    }

    private void StartBossMusic()
    {
        //StopMusic();
        _music.clip = _bossMusic;
    }

    private void StartMainMusic()
    {
        //StopMusic();
        _music.clip = _shopMusic;
    }

    public void PlayLoadingSound()
    {
        _sound.clip = _loading;
        _sound.Play();
    }
    public void PlayTreatSound(int arg0)
    {
        _sound.clip = _treat;
        _sound.Play();
    }

    public void PlayMergeSound(int arg0, int arg1)
    {
        _sound.clip = _merge;
        _sound.Play();
    }

    public void PlayBlackMouseSound()
    {
        _sound.clip = _blackMouse;
        _sound.Play();
    }

    public void PlayButtonSound()
    {
        _sound.clip = _button;
        _sound.Play();
    }

    public void PlayNewRoomOrCatSound()
{
        _sound.clip = _newRoomOrCat;
        _sound.Play();
    }

    public void PlayMouseSound()
{
        _sound.clip = _mouse;
        _sound.Play();
    }

    public void PlayChaseSound()
{
        _sound.clip = _chase;
        _sound.Play();
    }

    public void PlayCatBoxSound()
{
        _sound.clip = _catBox;
        _sound.Play();
    }

    public void PlayAddCatSound()
{
        _sound.clip = _addCat;
        _sound.Play();
    }

    public void PlayFightSound()
{
        _sound.clip = _fight;
        _sound.Play();
    }

    public void PlayCatFullSound()
{
        _sound.clip = _catFull;
        _sound.Play();
    }

    public void PlayDragCoinSound()
{
        _sound.clip = _dragCoin;
        _sound.Play();
    }

    public void PlayDropCoinSound()
{
        _sound.clip = _dropCoin;
        _sound.Play();
    }

    public void PlayFullSound()
{
        _sound.clip = _full;
        _sound.Play();
    }



}
