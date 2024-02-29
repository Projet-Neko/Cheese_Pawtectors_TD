using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private Wave _waveSpawner;
    private Timer _timer;
    private bool _restartWait;
    #region Modules
    [SerializeField] private M_Entities _entities;
    [SerializeField] private M_Economy _economy;

    // EntitiesMod
    public CatSO[] Cats => _entities.Cats;
    public MouseSO[] Mouses => _entities.Mouses;
    public int MouseLevel => _entities.MouseLevel;
    public bool CanSpawnAlbino => _entities.CanSpawnAlbino;

    public void AlbinoHasSpawned() => _entities.AlbinoHasSpawned();

    // EconomyMod
    public int Meat => _economy.Meat;

    public void AddMeat(int amount) => _economy.AddMeat(amount);
    public void RemoveMeat(int amount) => _economy.RemoveMeat(amount);
    #endregion

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
        Debug.Log("Game Manager created.");
        _entities.Init();
        _economy.Init();
    }

    private void Start()
    {
        InitWave();
        Cheese.CheeseDeath += StopWave;
        _timer = new Timer(3f);
    }

    private void InitWave()
    {

    }

    private void Update()
    {
        if (_restartWait)
        {

            if (_timer.HasEnded())
            {
                RestartWave();
            }
        }
    }


    private void StopWave()
    {
        _waveSpawner.ReloadWave();
        _timer.Start();
        _restartWait = true;   
    }
    private void RestartWave()
    {
        _waveSpawner.StartWave();
        //autre feedback ?
        _restartWait = false;

    }

}