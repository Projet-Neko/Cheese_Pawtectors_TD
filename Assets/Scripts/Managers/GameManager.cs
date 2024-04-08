using PlayFab;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// TODO -> vérifier si le jeu est à jour

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private List<Module> _modules;

    [Header("Loading Bar")]
    [SerializeField] private Slider _loadingSlider;
    [SerializeField] private TMP_Text _loadingText;

    // --- Requests events ---
    public static event Action<string> OnError;
    public static event Action<string> OnSuccessMessage;
    public static event Action OnObsoleteVersion;

    // --- Loading events ---
    public static event Action OnInitComplete;
    public static event Action OnLoadingStart;
    public static event Action OnLoadingEnd;
    public static event Action OnRequest;
    public static event Action OnEndRequest;

    // --- Requests ---
    private int _requests;
    public string Token { get; set; }
    public PlayFab.ClientModels.EntityKey Entity => Mod<Mod_Account>().Entity;

    // --- Datas ---
    public Data Data => _data;
    private Data _data;

    // --- Scenes ---
    public bool IsPopupSceneLoaded => !string.IsNullOrEmpty(_popupSceneName);
    public string PopupSceneName => _popupSceneName;

    private string _popupSceneName;
    private bool _hasLoginPopupLoad;

    private bool _isInitCompleted = false;

    #region Modules
    // EntitiesMod
    public CatSO[] Cats => Mod<Mod_Entities>().Cats;
    public MouseSO[] Mouses => Mod<Mod_Entities>().Mouses;
    public Cheese Cheese => Mod<Mod_Entities>().Cheese;
    public bool CanSpawnAlbino => Mod<Mod_Entities>().CanSpawnAlbino;

    public void AlbinoHasSpawned() => Mod<Mod_Entities>().AlbinoHasSpawned();

    // WaveMod
    public int KilledEnemiesNumber => Mod<Mod_Waves>().KilledEnemiesNumber;
    public int MaxEnemyNumber => Mod<Mod_Waves>().MaxEnemyNumber;
    public int SpawnTime => Mod<Mod_Waves>().SpawnTime;

    public bool IsBossWave() => Mod<Mod_Waves>().IsBossWave();

    // EconomyMod
    public List<int> CatPrices => Mod<Mod_Economy>().CatPrices;

    public int MeatPerSecond() => Mod<Mod_Economy>().MeatPerSecond();
    public int GetCheapestCatIndex() => Mod<Mod_Economy>().GetCheapestCatIndex();
    public void AddCurrency(Currency currency, int amount) => Mod<Mod_Economy>().AddCurrency(currency, amount);
    public void RemoveCurrency(Currency currency, int amount) => Mod<Mod_Economy>().RemoveCurrency(currency, amount);

    // AccountMod
    public bool IsLoggedIn => Mod<Mod_Account>().IsLoggedIn;
    public DateTime? LastLogin => Mod<Mod_Account>().LastLogin;
    public string Username => Mod<Mod_Account>().Username;
    public bool HasChangedUsername => Mod<Mod_Account>().HasChangedUsername;

    public void UpdateUsername(string username) => StartCoroutine(Mod<Mod_Account>().UpdateUsername(username));
    #endregion

    private T Mod<T>() where T : Module => _modules.OfType<T>().First();

    private void Start()
    {
        if (!CreateInstance()) return;
        _loadingSlider.value = 0;

        Module.OnInitComplete += Module_OnInitComplete;
        SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;

        StartCoroutine(CheckGameVersion());
    }

    private IEnumerator CheckGameVersion()
    {
        yield return StartAsyncRequest();

        PlayFabClientAPI.LoginWithCustomID(new()
        {
            CustomId = "CheckingVersion",
            CreateAccount = true,
        }, res =>
        {
            PlayFabClientAPI.GetTitleData(new(), res =>
            {
                EndRequest();

                if (res.Data["Version"] != Application.version)
                {
                    Debug.LogError("Version is obsolete !");
                    OnObsoleteVersion?.Invoke();
                    return;
                }

                Init();
            }, OnRequestError);
        }, OnRequestError);
    }

    private void Init()
    {
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
        Mod<Mod_Audio>().Init(this);

        FindObjectOfType<Mod_Audio>().StartTitleMusic();
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode != LoadSceneMode.Additive) return;
        //if (IsPopupSceneLoaded) SceneManager.UnloadSceneAsync(_popupSceneName);
        _popupSceneName = scene.name;
    }

    private void SceneManager_sceneUnloaded(Scene scene)
    {
        if (scene.name != _popupSceneName) return;
        _popupSceneName = null;
    }

    private void Module_OnInitComplete(Type mod)
    {
        _loadingSlider.value += Mathf.Ceil(100.0f / _modules.Count);
        _loadingText.text = _loadingSlider.value.ToString() + "%";

        if (mod == typeof(Mod_Account))
        {
            Mod<Mod_Economy>().Init(this);
            _loadingSlider.gameObject.SetActive(true);
        }

        else if (mod == typeof(Mod_Economy)) Mod<Mod_Clans>().Init(this);
        else if (mod == typeof(Mod_Clans)) StartCoroutine(CompleteInit());

        FindObjectOfType<Mod_Audio>().StartMainMusic();
    }

    private void OnDestroy()
    {
        Module.OnInitComplete -= Module_OnInitComplete;

        // Data events
        StorageSlot.OnSlotChanged -= (slotIndex, catIndex) => _data.UpdateStorage(slotIndex, catIndex);
        Merge.OnCatMerge -= (slotIndex, catIndex) => _data.UpdateStorage(slotIndex, catIndex);
        Discard.OnCatDiscard -= (slotIndex, catIndex) => _data.UpdateStorage(slotIndex, catIndex);
        CatBoxSpawner.OnBoxSpawn -= (slotIndex) => _data.UpdateStorage(slotIndex, -2);
        Storage.OnCatSpawn -= (slotIndex, catIndex, free) => _data.AdoptCat(catIndex - 1, slotIndex, free);
    }

    private bool CreateInstance()
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
            foreach (var currency in _data.Currencies) yield return Mod<Mod_Economy>().UpdateCurrency((Currency)currency.Index);
            yield return Mod<Mod_Account>().UpdateData();
        }
    }

    private void DebugOnly()
    {
        //DeleteAccountData();
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

    public void DeleteAccountData()
    {
        _data = new();
        Mod<Mod_Economy>().UpdateCatPrices();
        StartCoroutine(Mod<Mod_Account>().UpdateData());
    }
}