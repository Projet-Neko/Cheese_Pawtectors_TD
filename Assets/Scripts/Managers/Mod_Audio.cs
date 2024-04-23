using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

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
    }

    private void OnDestroy()
    {
        //Mod_Waves.BossWave -= StartBossMusic;
        Mod_Waves.OnBossDefeated -= StartMainMusic;
    
    }

    public override void Init(GameManager gm)
    {
        base.Init(gm);
        InitComplete();
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
        => _titleScreen.PlayOneShot(_loading);

    public void PlayMeatSound()
        => _titleScreen.PlayOneShot(_meat);

    public void PlayMergeSound()
        => _titleScreen.PlayOneShot(_merge);

    public void PlayAlbinosSound()
        => _titleScreen.PlayOneShot(_albinos);

    public void PlayButtonSound()
        => _titleScreen.PlayOneShot(_button);

    public void PlayNewRoomOrCatSound()
        => _titleScreen.PlayOneShot(_newRoomOrCat);

    public void PlayMouseSound()
        => _titleScreen.PlayOneShot(_mouse);

    public void PlayChaseSound()
        => _titleScreen.PlayOneShot(_chase);

    public void PlayCatBoxSound()
        => _titleScreen.PlayOneShot(_catBox);

    public void PlayAddCatSound()
        => _titleScreen.PlayOneShot(_addCat);

    public void PlayFightSound()
        => _titleScreen.PlayOneShot(_fight);

    public void PlayCatFullSound()
        => _titleScreen.PlayOneShot(_catFull);

    public void PlayDragCoinSound()
        => _titleScreen.PlayOneShot(_dragCoin);

    public void PlayDropCoinSound()
        => _titleScreen.PlayOneShot(_dropCoin);

    public void PlayFullSound()
        => _titleScreen.PlayOneShot(_full);



}
