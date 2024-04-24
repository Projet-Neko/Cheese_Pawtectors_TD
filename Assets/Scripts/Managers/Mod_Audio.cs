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
        //Mod_Waves.BossWave += StartBossMusic;
        /* Mod_Waves.OnBossDefeated += StartMainMusic;  
         Volume.OnMusicVolumeChange += SetMusicVolume;  
         Volume.OnSoundVolumeChange += SetSoundVolume;  
         Mod_Economy.treatWin += PlayTreatSound;  
         Merge.OnCatMerge += PlayMergeSound;  
         Mod_Waves.BlackMouseSpawned += PlayBlackMouseSound;  
         Button.OnButtonClicked += PlayButtonSound;  */
    }

    private void OnDestroy()
    {
        //Mod_Waves.BossWave -= StartBossMusic;
        /*Mod_Waves.OnBossDefeated -= StartMainMusic;
        Volume.OnMusicVolumeChange -= SetMusicVolume;
        Volume.OnSoundVolumeChange -= SetSoundVolume;
        Mod_Economy.treatWin -= PlayTreatSound;
        Merge.OnCatMerge -= PlayMergeSound;
        Mod_Waves.BlackMouseSpawned -= PlayBlackMouseSound;
        Button.OnButtonClicked -= PlayButtonSound;*/
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
                break;

            case "Build":
                //StopMusic();
                _music.clip = _buildMusic;
                _previousScene = sceneName;
                break;

            case "Catalog":
                //StopMusic();
                _music.clip = _shopMusic;
                _previousAdditiveScene = sceneName;
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

    /*private void //StopMusic()
    {
        _titleScreen.Stop();
        _mainScreen.Stop();
        _build.Stop();
        _shop.Stop();
        _boss.Stop();
    }*/

    public void PlayLoadingSound()
    => _sound.clip = _loading;    

    public void PlayTreatSound(int arg0)
    => _sound.clip = _treat;  

    public void PlayMergeSound(int arg0, int arg1)
        => _sound.clip = _merge;

    public void PlayBlackMouseSound()
        => _sound.clip = _blackMouse;

    public void PlayButtonSound()
        => _sound.clip = _button;

    public void PlayNewRoomOrCatSound()
        => _sound.clip = _newRoomOrCat;

    public void PlayMouseSound()
        => _sound.clip = _mouse;

    public void PlayChaseSound()
        => _sound.clip = _chase;

    public void PlayCatBoxSound()
        => _sound.clip = _catBox;

    public void PlayAddCatSound()
        => _sound.clip = _addCat;

    public void PlayFightSound()
             => _sound.clip = _fight;

    public void PlayCatFullSound()
        => _sound.clip = _catFull;

    public void PlayDragCoinSound()
        => _sound.clip = _dragCoin;

    public void PlayDropCoinSound()
        => _sound.clip = _dropCoin;

    public void PlayFullSound()
        => _sound.clip = _full;



}
