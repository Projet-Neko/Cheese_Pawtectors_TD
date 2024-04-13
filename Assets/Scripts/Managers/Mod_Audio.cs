using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Mod_Audio : Module
{
    [SerializeField] AudioSource _music;
    [SerializeField] AudioSource _soundEffect;

    Dictionary<string, AudioClip> _musicsClip = new Dictionary<string, AudioClip>();

    [Header("Musics")]
    [SerializeField] private AudioClip _titleScreen;
    [SerializeField] private AudioClip _mainScreen;
    [SerializeField] private AudioClip _build;
    [SerializeField] private AudioClip _boss;
    [SerializeField] private AudioClip _currencyShop;

    private void Awake()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        _musicsClip.Add("TitleScreen", _titleScreen);
        _musicsClip.Add("MainScreen", _mainScreen);
        _musicsClip.Add("Build", _build);
        _musicsClip.Add("Boss", _boss);
        _musicsClip.Add("Currency Shop", _currencyShop);
    }
    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!_musicsClip.ContainsKey(scene.name)) return;
        _music.Stop();
        _music.clip = _musicsClip[scene.name];
        _music.Play();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    public void SoundEffect(AudioClip clip)
    {
        _soundEffect.PlayOneShot(clip);
    }
}
