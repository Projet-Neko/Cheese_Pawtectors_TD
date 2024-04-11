using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class Mod_Audio : Module
{
    AudioSource _audioSource;

    List<string> _musics = new List<string>() {"TitleScreen", "MainScreen", "Build", "Shop"};

    [Header("Musics")]
    [SerializeField] private AudioSource _titleScreen;
    [SerializeField] private AudioSource _mainScreen;
    [SerializeField] private AudioSource _build;
    [SerializeField] private AudioSource _shop;

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
        _audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }
    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == _musics[0])
        {
            if (!_titleScreen.isPlaying)
            {
                _titleScreen.Play();
                _mainScreen.Stop();
                _build.Stop();
                _shop.Stop();
            }
        }

        if (arg0.name == _musics[1])
        {
            if (!_mainScreen.isPlaying)
            {
                _titleScreen.Stop();
                _mainScreen.Play();
                _build.Stop();
                _shop.Stop();
            }
        }

        if (arg0.name == _musics[2])
        {
            if (!_build.isPlaying)
            {
                _titleScreen.Stop();
                _mainScreen.Stop();
                _build.Play();
                _shop.Stop();
            }
        }

        if (arg0.name == _musics[3])
        {
            if (!_shop.isPlaying)
            {
                _titleScreen.Stop();
                _mainScreen.Stop();
                _build.Stop();
                _shop.Play();
            }
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }
    
    /*public void LoadingSound()
    {
        _audioSource.clip = _loading;
        _audioSource.Play();
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
    }
    public void ButtonSound()
    {
        _audioSource.clip = _button;
        _audioSource.Play();
    }
    public void NewRoomOrCatSound()
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
