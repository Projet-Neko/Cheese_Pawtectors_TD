using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class Mod_Audio : Module
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioSource _music;
    [SerializeField] AudioSource _soundEffect;

    List<string> _sceneMusic = new List<string>() {"TitleScreen", "MainScreen", "Build", "Currency Shop"};

    Dictionary<string, AudioClip> _musicsClip = new Dictionary<string, AudioClip>();

    [Header("Musics")]
    /*[SerializeField] private AudioSource _titleScreen;
    [SerializeField] private AudioSource _mainScreen;
    [SerializeField] private AudioSource _build;
    [SerializeField] private AudioSource _currencyShop;*/

    [SerializeField] private AudioClip _titleScreen;
    [SerializeField] private AudioClip _mainScreen;
    [SerializeField] private AudioClip _build;
    [SerializeField] private AudioClip _currencyShop;

    /*[Header("SoundEffects")]
    public AudioClip _loading;
    public AudioClip _meat;
    public AudioClip _merge;
    public AudioClip _albinos;
    public AudioClip _button;
    public AudioClip _newRoomOrCat;
    public AudioClip _mouse;
    public AudioClip _chase;
    public AudioClip _catBox;
    public AudioClip _addCat;
    public AudioClip _fight;
    public AudioClip _catFull;
    public AudioClip _dragCoin;
    public AudioClip _dropCoin;
    public AudioClip _full;*/

    private void Awake()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        _musicsClip.Add("TitleScreen", _titleScreen);
        //Ajouter pour les autres (element de la liste)
    }
    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        _music.Stop();
        _music.clip = _musicsClip[arg0.name];
        _music.Play();

        /*if (arg0.name == _sceneMusic[0])
        {
            if (!_titleScreen.isPlaying)
            {
                _titleScreen.Play();
                _mainScreen.Stop();
                _build.Stop();
                _currencyShop.Stop();
            }
        }

        if (arg0.name == _sceneMusic[1])
        {
            if (!_mainScreen.isPlaying)
            {
                _titleScreen.Stop();
                _mainScreen.Play();
                _build.Stop();
                _currencyShop.Stop();
            }
        }

        if (arg0.name == _sceneMusic[2])
        {
            if (!_build.isPlaying)
            {
                _titleScreen.Stop();
                _mainScreen.Stop();
                _build.Play();
                _currencyShop.Stop();
            }
        }

        if (arg0.name == _sceneMusic[3])
        {
            if (!_currencyShop.isPlaying)
            {
                _titleScreen.Stop();
                _mainScreen.Stop();
                _build.Stop();
                _currencyShop.Play();
            }
        }*/
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }
    
    /*public void LoadingSound()
    {
        _audioSource.PlayOneShot(_loading);
    }
    public void MeatSound()
    {
        _audioSource.clip = _meat;
        _audioSource.Play();
    }
    public void MergeSound()
    {
        _audioSource.clip = _merge;
        _audioSource.Play();
    }
    public void AlbinosSound()
    {
        _audioSource.clip = _albinos;
        _audioSource.Play();
    }*/
    public void SoundEffect(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
    /*public void NewRoomOrCatSound()
    {
        _audioSource.clip = _newRoomOrCat;
        _audioSource.Play();
    }
    public void MouseSound()
    {
        _audioSource.clip = _mouse;
        _audioSource.Play();
    }
    public void ChaseSound()
    {
        _audioSource.clip = _chase;
        _audioSource.Play();
    }
    public void CatBoxSound()
    {
        _audioSource.clip = _catBox;
        _audioSource.Play();
    }
    public void AddCatSound()
    {
        _audioSource.clip = _addCat;
        _audioSource.Play();
    }
    public void FightSound()
    {
        _audioSource.clip = _fight;
        _audioSource.Play();
    }
    public void CatFullSound()
    {
        _audioSource.clip = _catFull;
        _audioSource.Play();
    }
    public void DragCoinSound()
    {
        _audioSource.clip = _dragCoin;
        _audioSource.Play();
    }
    public void DropCoinSound()
    {
        _audioSource.clip = _dropCoin;
        _audioSource.Play();
    }
    public void FullSound()
    {
        _audioSource.clip = _full;
        _audioSource.Play();
    }*/
}
