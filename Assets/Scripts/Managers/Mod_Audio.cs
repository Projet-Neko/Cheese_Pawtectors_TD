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
    {
        _music.volume = volume;
    } 

    public void SetSoundVolume(float volume)
    {
        _sound.volume = volume;
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
    //=> _titleScreen.PlayOneShot(_loading, _soundVolume); //Start du jeu mais à changer ?
    //=>_sound.PlayOneShot(_loading, _soundVolume);
    {
        _music.PlayOneShot(_loading, _soundVolume);
    }

    public void PlayTreatSound(int arg0)
    { }
    //=> _sound.PlayOneShot(_treat, _soundVolume);  

    public void PlayMergeSound(int arg0, int arg1)
        => _sound.PlayOneShot(_merge, _soundVolume);  

    public void PlayBlackMouseSound()
        => _sound.PlayOneShot(_blackMouse, _soundVolume);  

    public void PlayButtonSound()
        => _sound.PlayOneShot(_button, _soundVolume);
        //=> _sound.PlayOneShot(_button);

    public void PlayNewRoomOrCatSound()
        => _sound.PlayOneShot(_newRoomOrCat, _soundVolume);

    public void PlayMouseSound()
        => _sound.PlayOneShot(_mouse, _soundVolume);

    public void PlayChaseSound()
        => _sound.PlayOneShot(_chase, _soundVolume);

    public void PlayCatBoxSound()
        => _sound.PlayOneShot(_catBox, _soundVolume);

    public void PlayAddCatSound()
        => _sound.PlayOneShot(_addCat, _soundVolume);

    public void PlayFightSound()
        => _sound.PlayOneShot(_fight, _soundVolume);

    public void PlayCatFullSound()
        => _sound.PlayOneShot(_catFull, _soundVolume);

    public void PlayDragCoinSound()
        => _sound.PlayOneShot(_dragCoin, _soundVolume);

    public void PlayDropCoinSound()
        => _sound.PlayOneShot(_dropCoin, _soundVolume);

    public void PlayFullSound()
        => _sound.PlayOneShot(_full, _soundVolume);



}
