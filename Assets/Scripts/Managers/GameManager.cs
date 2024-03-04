using PlayFab;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // --- Requests events ---
    public static event Action<PlayFabError> OnError;
    public static event Action<string> OnSuccessMessage;

    // --- Loading events ---
    public static event Action OnLoadingStart;
    //public static event Action OnBigLoadingStart;
    public static event Action OnLoadingEnd;
    public static event Action OnRequest;
    public static event Action OnEndRequest;

    // --- Requests ---
    private int _requests;
    public string Token { get; set; }
    public PlayFab.ClientModels.EntityKey Entity => _account.Entity;
    //public bool AccountChecked { get; set; }
    //public bool DailiesCheck { get; set; }
    //public bool IsObsolete { get; private set; }

    // --- Datas ---
    public Data Data => _data;
    private readonly Data _data = new();

    #region Modules
    [Header("Modules")]
    [SerializeField] private Mod_Entities _entities;
    [SerializeField] private Mod_Economy _economy;
    [SerializeField] private Mod_Wave _wave;
    [SerializeField] private Mod_Account _account;

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

    // AccountMod
    public static event Action OnLoginSuccess;
    #endregion

    private void Awake()
    {
        if (!Init()) return;

        _entities.Init(this);
        _economy.Init(this);
        _wave.Init(this);
        _account.Init(this);
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

    #region AccountMod
    public void InvokeOnLoginSuccess() => OnLoginSuccess?.Invoke();
    #endregion

    #region Database Requests
    public IEnumerator StartAsyncRequest(string log = null)
    {
        int request = StartRequest(log);
        //Debug.Log($"new async request received - {_requests} requests remaining.");
        if (_requests > 1) yield return new WaitUntil(() => _requests == request);
        //Debug.Log($"starting async request... - {_requests} requests remaining.");
    }

    public int StartRequest(string log = null)
    {
        OnLoadingStart?.Invoke();
        OnRequest?.Invoke();
        int currentRequest = _requests;
        _requests++;
        if (!string.IsNullOrEmpty(log)) Debug.Log(log);
        return currentRequest;
    }

    public void EndRequest(string log = null)
    {
        OnLoadingEnd?.Invoke();
        OnEndRequest?.Invoke();
        _requests--;

        if (!string.IsNullOrEmpty(log))
        {
            Debug.Log(log);
            OnSuccessMessage?.Invoke(log);
        }
    }

    public void OnRequestError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
        OnError?.Invoke(error);
        OnLoadingEnd?.Invoke();
    }
    #endregion
}