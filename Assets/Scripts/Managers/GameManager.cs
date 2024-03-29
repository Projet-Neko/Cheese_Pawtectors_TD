using PlayFab;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// TODO -> v�rifier si le jeu est � jour

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Slider _loadingSlider;

    // --- Requests events ---
    public static event Action<string> OnError;
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
    public PlayFab.ClientModels.EntityKey Entity => Mod<Mod_Account>().Entity;
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
    private bool _hasLoginPopupLoad;

    private bool _isInitCompleted = false;

    #region Modules
    [SerializeField] private List<Module> _modules;

    // EntitiesMod
    public CatSO[] Cats => Mod<Mod_Entities>().Cats;
    public MouseSO[] Mouses => Mod<Mod_Entities>().Mouses;
    public Cheese Cheese => Mod<Mod_Entities>().Cheese;
    public bool CanSpawnAlbino => Mod<Mod_Entities>().CanSpawnAlbino;

    public void AlbinoHasSpawned() => Mod<Mod_Entities>().AlbinoHasSpawned();

    // WaveMod
    public int SpawnTime => Mod<Mod_Waves>().SpawnTime;

    public bool IsBossWave() => Mod<Mod_Waves>().IsBossWave();

    // EconomyMod
    public Dictionary<Currency, int> Currencies => Mod<Mod_Economy>().Currencies;
    public List<int> CatPrices => Mod<Mod_Economy>().CatPrices;

    public int GetCheapestCatIndex() => Mod<Mod_Economy>().GetCheapestCatIndex();
    public void AddCurrency(Currency currency, int amount) => Mod<Mod_Economy>().AddCurrency(currency, amount);
    public void RemoveCurrency(Currency currency, int amount) => Mod<Mod_Economy>().RemoveCurrency(currency, amount);

    // AccountMod
    public DateTime? LastLogin => Mod<Mod_Account>().LastLogin;
    public bool IsLoggedIn => Mod<Mod_Account>().IsLoggedIn;
    #endregion

    private T Mod<T>() where T : Module => _modules.OfType<T>().First();

    private void Awake()
    {
        if (!Init()) return;

        Module.OnInitComplete += Module_OnInitComplete;
        SceneLoader.OnPopupSceneToogle += SceneLoader_OnPopupSceneToogle;

        // Init all entities SO
        Mod<Mod_Entities>().Init(this);

        // Init local data file - entities init required first
        _data = new();

        // Data events
        StorageSlot.OnSlotChanged += (slotIndex, catIndex) => _data.UpdateStorage(slotIndex, catIndex);
        Merge.OnCatMerge += (slotIndex, catIndex) => _data.UpdateStorage(slotIndex, catIndex);
        Discard.OnCatDiscard += (slotIndex, catIndex) => _data.UpdateStorage(slotIndex, catIndex);
        CatBoxSpawner.OnBoxSpawn += (slotIndex) => _data.UpdateStorage(slotIndex, -2);
        Storage.OnCatSpawn += (slotIndex, catIndex, free) => _data.AdoptCat(catIndex - 1, slotIndex, free);
        Cat.OnUnlock += _data.UnlockCat;

        Mod<Mod_Waves>().Init(this);
        Mod<Mod_Leaderboards>().Init(this);
        Mod<Mod_Account>().Init(this);
    }

    private void Module_OnInitComplete(Type mod)
    {
        if (mod == typeof(Mod_Account)) Mod<Mod_Economy>().Init(this);
        else if (mod == typeof(Mod_Economy)) Mod<Mod_Clans>().Init(this);
        else if (mod == typeof(Mod_Clans)) StartCoroutine(CompleteInit());
    }

    private void OnDestroy()
    {
        Module.OnInitComplete -= Module_OnInitComplete;
        SceneLoader.OnPopupSceneToogle -= SceneLoader_OnPopupSceneToogle;

        // Data events
        StorageSlot.OnSlotChanged -= (slotIndex, catIndex) => _data.UpdateStorage(slotIndex, catIndex);
        Merge.OnCatMerge -= (slotIndex, catIndex) => _data.UpdateStorage(slotIndex, catIndex);
        Discard.OnCatDiscard -= (slotIndex, catIndex) => _data.UpdateStorage(slotIndex, catIndex);
        CatBoxSpawner.OnBoxSpawn -= (slotIndex) => _data.UpdateStorage(slotIndex, -2);
        Storage.OnCatSpawn -= (slotIndex, catIndex, free) => _data.AdoptCat(catIndex - 1, slotIndex, free);
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
    private void SceneLoader_OnPopupSceneToogle(bool isPopupSceneLoaded, string popupName)
    {
        _isPopupSceneLoaded = isPopupSceneLoaded;
        _popupSceneName = popupName;
    }
    #endregion

    public bool HasLoginPopupLoad()
    {
        if (_hasLoginPopupLoad) return true;
        _hasLoginPopupLoad = true;
        return false;
    }

    private IEnumerator CompleteInit()
    {
        if (LastLogin == null) yield return Mod<Mod_Account>().UpdateData();
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
            foreach (var currency in Currencies) yield return Mod<Mod_Economy>().UpdateCurrency(currency.Key);
            yield return Mod<Mod_Account>().UpdateData();
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
    public void OnRequestError(string error)
    {
        Debug.LogError(error);
        OnError?.Invoke(error);
        EndRequest();
    }
    public void OnRequestError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
        OnError?.Invoke(error.GenerateErrorReport());
        EndRequest();
    }
    #endregion

    public void DeleteLocalDatas()
    {
        _data = new();
        _data.Update();
    }
}