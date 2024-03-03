using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    #region Modules
    [Header("Modules")]
    [SerializeField] private M_Entities _entities;
    [SerializeField] private M_Economy _economy;
    [SerializeField] private M_Wave _wave;

    // EntitiesMod
    public CatSO[] Cats => _entities.Cats;
    public MouseSO[] Mouses => _entities.Mouses;
    public Cheese Cheese => _entities.Cheese;
    public int MouseLevel => _entities.MouseLevel;
    public bool CanSpawnAlbino => _entities.CanSpawnAlbino;

    public void AlbinoHasSpawned() => _entities.AlbinoHasSpawned();

    // EconomyMod
    public int Meat => _economy.Meat;
    public List<int> CatPrices => _economy.CatPrices;

    public int GetCheapestCatIndex() => _economy.GetCheapestCatIndex();
    public bool CanAdopt(int catLevel) => _economy.CanAdopt(catLevel);
    public void AddMeat(int amount) => _economy.AddMeat(amount);
    public void RemoveMeat(int amount) => _economy.RemoveMeat(amount);
    #endregion

    private void Awake()
    {
        if (!Init()) return;

        _entities.Init();
        _economy.Init();
        _wave.Init();

        _wave.StartWave();
    }

    private bool Init()
    {
        if (Instance != null)
        {
            Destroy(this);
            return false;
        }

        Instance = this;
        DontDestroyOnLoad(this);
        Debug.Log("Game Manager created.");
        return true;
    }
}