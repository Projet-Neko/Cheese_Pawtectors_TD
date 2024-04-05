using UnityEngine;
using System.Linq;

public class Mod_Audio : Module
{
    [Header("Musics")]
    [SerializeField] private AudioSource _titleScreen;
    [SerializeField] private AudioSource _mainScreen;
    [SerializeField] private AudioSource _build;
    [SerializeField] private AudioSource _shop;
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


    private bool _titleIsPlaying = false;
    private bool _mainIsPlaying = false;
    private bool _buildIsPlaying = false;
    private bool _shopIsPlaying = false;
    private bool _bossIsPlaying = false;
    

    public void StartTitleMusic()
    {
        if (!_titleIsPlaying)
        {
            _titleScreen.Play();
            _mainScreen.Stop();
            _build.Stop();
            _shop.Stop();
            _boss.Stop();
            _titleIsPlaying = true;
            _mainIsPlaying = false;
            _buildIsPlaying = false;
            _shopIsPlaying = false;
            _bossIsPlaying = false;
        }
    }

    public void StartMainMusic()
    {
        if (!_mainIsPlaying)
        {
            _titleScreen.Stop();
            _mainScreen.Play();
            _build.Stop();
            _shop.Stop();
            _boss.Stop();
            _titleIsPlaying = false;
            _mainIsPlaying = true;
            _buildIsPlaying = false;
            _shopIsPlaying = false;
            _bossIsPlaying = false;
        }
    }
    public void StartBuildMusic()
    {
        if (!_buildIsPlaying)
        {
            _titleScreen.Stop();
            _mainScreen.Stop();
            _build.Play();
            _shop.Stop();
            _boss.Stop();
            _titleIsPlaying = false;
            _mainIsPlaying = false;
            _buildIsPlaying = true;
            _shopIsPlaying = false;
            _bossIsPlaying = false;
        }
    }
    public void StartShopMusic()
    {
        if (!_shopIsPlaying)
        {
            _titleScreen.Stop();
            _mainScreen.Stop();
            _build.Stop();
            _shop.Play();
            _boss.Stop();
            _titleIsPlaying = false;
            _mainIsPlaying = false;
            _buildIsPlaying = false;
            _shopIsPlaying = true;
            _bossIsPlaying = false;
        }
    }
    public void StartBossMusic()
    {
        if (!_bossIsPlaying)
        {
            _titleScreen.Stop();
            _mainScreen.Stop();
            _build.Stop();
            _shop.Stop();
            _boss.Play();
            _titleIsPlaying = false;
            _mainIsPlaying = false;
            _buildIsPlaying = false;
            _shopIsPlaying = false;
            _bossIsPlaying = true;
        }
    }
}
