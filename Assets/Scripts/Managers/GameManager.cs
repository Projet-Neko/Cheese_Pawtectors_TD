using PlayFab;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO -> vérifier si le jeu est à jour

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // --- Requests events ---
    public static event Action<PlayFabError> OnError;
    public static event Action<string> OnSuccessMessage;

    // --- Loading events ---
    public static event Action OnInitComplete;
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
    //public bool IsObsolete { get; private set; }

    // --- Datas ---
    public Data Data => _data;
    private Data _data;

    // --- Scenes ---
    public bool IsPopupSceneLoaded => _isPopupSceneLoaded;
    public string PopupSceneName => _popupSceneName;

    private bool _isPopupSceneLoaded;
    private string _popupSceneName;

    private bool _isInitCompleted = false;

    #region Modules
    [Header("Modules")]
    [SerializeField] private Mod_Entities _entities;
    [SerializeField] private Mod_Economy _economy;
    [SerializeField] private Mod_Waves _wave;
    [SerializeField] private Mod_Account _account;
    [SerializeField] private Mod_Clans _clans;

    // EntitiesMod
    public CatSO[] Cats => _entities.Cats;
    public MouseSO[] Mouses => _entities.Mouses;
    public Cheese Cheese => _entities.Cheese;
    public int MouseLevel => _entities.MouseLevel;
    public bool CanSpawnAlbino => _entities.CanSpawnAlbino;

    public void AlbinoHasSpawned() => _entities.AlbinoHasSpawned();
    public int GetLastUnlockedCatLevel() => _entities.GetLastUnlockedCatLevel();
    public bool IsBossWave() => _wave.IsBossWave();

    // WaveMod
    public int SpawnTime => _wave.SpawnTime;

    // EconomyMod
    public Dictionary<Currency, int> Currencies => _economy.Currencies;
    public List<int> CatPrices => _economy.CatPrices;

    public int GetCheapestCatIndex() => _economy.GetCheapestCatIndex();
    public void AddCurrency(Currency currency, int amount) => _economy.AddCurrency(currency, amount);
    public void RemoveCurrency(Currency currency, int amount) => _economy.RemoveCurrency(currency, amount);

    // AccountMod
    //public static event Action OnLoginSuccess;
    public DateTime? LastLogin => _account.LastLogin;
    public bool IsLoggedIn => _account.IsLoggedIn;
    #endregion

    private void Awake()
    {
        if (!Init()) return;
        _entities.Init(this);
        _wave.Init(this);
        _data = new();
        _account.Init(this);

        Mod_Account.OnInitComplete += Mod_Account_OnInitComplete;
        Mod_Economy.OnInitComplete += Mod_Economy_OnInitComplete;
        Mod_Clans.OnInitComplete += Mod_Clans_OnInitComplete;

        SceneLoader.OnPopupSceneToogle += SceneLoader_OnPopupSceneToogle;
        StorageSlot.OnSlotChanged += StorageSlot_OnSlotChanged;
        CatBoxSpawner.OnBoxSpawn += CatBoxSpawner_OnBoxSpawn;
        Storage.OnCatSpawn += Storage_OnCatSpawn;
    }

    private void Storage_OnCatSpawn(int catLevel, int slotIndex)
    {
        _data.AdoptCat(catLevel - 1, slotIndex);
    }

    private void CatBoxSpawner_OnBoxSpawn(int slotIndex)
    {
        _data.UpdateStorage(slotIndex, -2);
    }

    private void StorageSlot_OnSlotChanged(int slotIndex, int catIndex)
    {
        _data.UpdateStorage(slotIndex, catIndex);
    }

    private void SceneLoader_OnPopupSceneToogle(bool isPopupSceneLoaded, string popupName)
    {
        _isPopupSceneLoaded = isPopupSceneLoaded;
        _popupSceneName = popupName;
    }

    private void OnDestroy()
    {
        Mod_Account.OnInitComplete -= Mod_Account_OnInitComplete;
        Mod_Economy.OnInitComplete -= Mod_Economy_OnInitComplete;
        Mod_Clans.OnInitComplete -= Mod_Clans_OnInitComplete;

        SceneLoader.OnPopupSceneToogle -= SceneLoader_OnPopupSceneToogle;
        StorageSlot.OnSlotChanged -= StorageSlot_OnSlotChanged;
        CatBoxSpawner.OnBoxSpawn -= CatBoxSpawner_OnBoxSpawn;
        Storage.OnCatSpawn -= Storage_OnCatSpawn;
    }

    private bool Init()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return false;
        }

        Instance = this;
        DontDestroyOnLoad(this);
        Debug.Log("<color=yellow>Game Manager created.</color>");
        return true;
    }

    #region Gestion des events
    private void Mod_Account_OnInitComplete()
    {
        _economy.Init(this);
    }
    private void Mod_Economy_OnInitComplete()
    {
        _clans.Init(this);
    }
    private void Mod_Clans_OnInitComplete()
    {
        StartCoroutine(CompleteInit());
    }
    #endregion

    private IEnumerator CompleteInit()
    {
        if (LastLogin == null) yield return _account.UpdateData();
        Debug.Log("<color=yellow>----- GAME MANAGER INIT COMPLETED ! -----</color>");
        OnInitComplete?.Invoke();
        _isInitCompleted = true;
        DebugOnly();
        yield return StartUpdates();
    }

    private IEnumerator StartUpdates()
    {
        Debug.Log("<color=orange>Start game updates...</color>");

        while (true)
        {
            yield return new WaitForSeconds(60);
            Debug.Log("Starting auto save...");
            foreach (var currency in Currencies) yield return _economy.UpdateCurrency(currency.Key);
            yield return _account.UpdateData();
        }
    }

    private void DebugOnly()
    {
        //DeleteLocalDatas(); // Reset local datas
    }

    private void OnApplicationPause(bool pause)
    {
        if (!_isInitCompleted) return;
        Debug.Log("Updating local data on application pause...");
        Data.Update();
    }

    #region AccountMod
    //public void InvokeOnLoginSuccess() => OnLoginSuccess?.Invoke();
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
        if (!string.IsNullOrEmpty(log)) Debug.Log($"<color=orange>{log}</color>");
        return currentRequest;
    }
    public void EndRequest(string log = null)
    {
        OnLoadingEnd?.Invoke();
        OnEndRequest?.Invoke();
        _requests--;

        if (!string.IsNullOrEmpty(log))
        {
            Debug.Log($"<color=lime>{log}</color>");
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

    public void DeleteLocalDatas()
    {
        _data = new();
        _data.Update();
    }
}