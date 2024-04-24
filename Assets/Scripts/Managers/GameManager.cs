using NaughtyAttributes;
using PlayFab;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// TODO -> v�rifier si le jeu est � jour

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private List<Module> _modules;
    [SerializeField, Scene] private string _loadingPopup;

    [Header("Loading Bar")]
    [SerializeField] private Slider _loadingSlider;
    [SerializeField] private TMP_Text _loadingText;

    [Header("Scenes")]
    [SerializeField, Scene] private string _headBandScene;
    [SerializeField, Scene] private string _errorPopupScene;

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
    public string ErrorMessage => _errorMessage;

    private int _requests;
    public string Token { get; set; }
    public PlayFab.ClientModels.EntityKey Entity => Mod<Mod_Account>().Entity;
    private bool _isLoadingPopupOpen;

    private string _errorMessage;

    // --- Datas ---
    public Data Data => _data;
    private Data _data;

    // --- Scenes ---
    public bool IsPopupSceneLoaded => !string.IsNullOrEmpty(_popupSceneName);
    public string PopupSceneName => _popupSceneName;

    private string _popupSceneName;
    private bool _hasLoginPopupLoad;

    private bool _isInitCompleted = false;
    private bool _errorLoaded = false;

    #region Modules
    // EntitiesMod
    public GameObject MousePrefab => Mod<Mod_Entities>().MousePrefab;
    public GameObject BlackMousePrefab => Mod<Mod_Entities>().BlackMousePrefab;
    public GameObject MouseRatPrefab => Mod<Mod_Entities>().MouseRatPrefab;
    public GameObject MouseBallPrefab => Mod<Mod_Entities>().MouseBallPrefab;
    public GameObject CheesePrefab => Mod<Mod_Entities>().CheesePrefab;
    public CatSO[] Cats => Mod<Mod_Entities>().Cats;
    public MouseSO[] Mouses => Mod<Mod_Entities>().Mouses;
    public Cheese Cheese => Mod<Mod_Entities>().Cheese;
    public bool CanSpawnAlbino => Mod<Mod_Entities>().CanSpawnAlbino;
    public bool CanSpawnBlackMouse => Mod<Mod_Entities>().CanSpawnBlackMouse;

    public void AlbinoHasSpawned() => Mod<Mod_Entities>().AlbinoHasSpawned();
    public void BlackMouseHasSpawned() => Mod<Mod_Entities>().BlackMouseHasSpawned();
    public void SpawnCheese(Transform room) => Mod<Mod_Entities>().SpawnCheese(room);

    // WaveMod
    public int KilledEnemiesNumber => Mod<Mod_Waves>().KilledEnemiesNumber;
    public int MaxEnemyNumber => Mod<Mod_Waves>().MaxEnemyNumber;
    public int SpawnTime => Mod<Mod_Waves>().SpawnTime;

    public bool IsBossWave() => Mod<Mod_Waves>().IsBossWave();

    // EconomyMod
    public List<int> CatPrices => Mod<Mod_Economy>().CatPrices;

    public int TreatPerSecond() => Mod<Mod_Economy>().MeatPerSecond();
    public int GetCheapestCatIndex() => Mod<Mod_Economy>().GetCheapestCatIndex();
    public void AddCurrency(Currency currency, int amount) => Mod<Mod_Economy>().AddCurrency(currency, amount);
    public void RemoveCurrency(Currency currency, int amount) => Mod<Mod_Economy>().RemoveCurrency(currency, amount);
    public int MeatGainedOffline() => Mod<Mod_Economy>().MeatGainedOffline();

    // AccountMod
    public bool IsLoggedIn => Mod<Mod_Account>().IsLoggedIn;
    public bool IsFirstLogin => Mod<Mod_Account>().IsFirstLogin;
    public DateTime? LastLogin => Mod<Mod_Account>().LastLogin;
    public string Username => Mod<Mod_Account>().Username;
    public bool HasChangedUsername => Mod<Mod_Account>().HasChangedUsername;

    public void UpdateUsername(string username) => StartCoroutine(Mod<Mod_Account>().UpdateUsername(username));

    // ClanMod
    public ClanSO[] Clans => Mod<Mod_Clans>().ClansData;
    public Clan SetUserClan(int clan) => Mod<Mod_Clans>().SetUserClan(clan);
    #endregion

    // PowerUpMod
    public bool IsPowerUpActive(PowerUpType powerUpType) => Mod<Mod_PowerUp>().IsPowerUpActive(powerUpType);

    private T Mod<T>() where T : Module => _modules.OfType<T>().First();

    private void Start()
    {
        if (!CreateInstance()) return;

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
        DragAndDropHandler.OnSlotChanged += (slotIndex, catIndex) => _data.UpdateStorage(slotIndex, catIndex);
        Merge.OnCatMerge += (slotIndex, catIndex) => _data.UpdateStorage(slotIndex, catIndex);
        Discard.OnCatDiscard += (slotIndex, catIndex) => _data.UpdateStorage(slotIndex, catIndex);
        CatBoxSpawner.OnBoxSpawn += (slotIndex) => _data.UpdateStorage(slotIndex, -2);
        Storage.OnCatSpawn += (slotIndex, catIndex, free) => _data.AdoptCat(catIndex - 1, slotIndex, free);
        Cat.OnUnlock += _data.UnlockCat;

        Mod<Mod_Account>().Init(this);

        Mod<Mod_Audio>().PlayLoadingSound();
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Mod<Mod_Audio>().StartMusic(scene.name);

        if (scene.name == _headBandScene) return;

        if (scene.name == _errorPopupScene)
        {
            _errorLoaded = true;
            return;
        }

        if (mode != LoadSceneMode.Additive)
        {
            _popupSceneName = null;
            return;
        }

        //Debug.Log("<color=lime>enabling popup mode</color>");
        _popupSceneName = scene.name;

    }

    private void SceneManager_sceneUnloaded(Scene scene)
    {
        if (scene.name == _errorPopupScene)
        {
            _errorLoaded = false;
            return;
        }

        Mod<Mod_Audio>().StartMusic(scene.name);
        if (scene.name != _popupSceneName) return;
        //Debug.Log("<color=red>disabling popup mode</color>");
        _popupSceneName = null;
    }

    private void Module_OnInitComplete(Type mod)
    {
        _loadingSlider.value += Mathf.Ceil(100.0f / _modules.Count);

        if (mod == typeof(Mod_Account))
        {
            Mod<Mod_Waves>().Init(this);
            Mod<Mod_Audio>().Init(this);
            Mod<Mod_Economy>().Init(this);
        }
        else if (mod == typeof(Mod_Economy))
        {
            Mod<Mod_Leaderboards>().Init(this);
            Mod<Mod_Clans>().Init(this);
        }
        else if (mod == typeof(Mod_Clans)) StartCoroutine(CompleteInit());

        //Mod<Mod_Audio>().StartMainMusic();
   
    }

    private void OnDestroy()
    {
        Module.OnInitComplete -= Module_OnInitComplete;

        // Data events
        DragAndDropHandler.OnSlotChanged -= (slotIndex, catIndex) => _data.UpdateStorage(slotIndex, catIndex);
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
        Debug.Log("<color=yellow>First time loading Login Popup</color>");
        _hasLoginPopupLoad = true;
        return false;
    }

    private IEnumerator CompleteInit()
    {
        if (IsFirstLogin) yield return Mod<Mod_Account>().UpdateData();
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
            //foreach (var currency in _data.Currencies) yield return Mod<Mod_Economy>().UpdateCurrency((Currency)currency.Index);
            //yield return Mod<Mod_Account>().UpdateData();
        }
    }

    private void DebugOnly()
    {
        Debug.Log("debug of game manager");
        //DeleteLocalDatas(); // Reset local datas
    }

    private void OnApplicationPause(bool pause)
    {
        if (!_isInitCompleted) return;
        //Debug.Log("Updating local data on application pause...");
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

        if (_isInitCompleted && !_isLoadingPopupOpen)
        {
            _isLoadingPopupOpen = true;
            SceneManager.LoadSceneAsync(_loadingPopup, LoadSceneMode.Additive);
        }

        int currentRequest = _requests;
        _requests++;
        if (!string.IsNullOrEmpty(log)) Debug.Log($"<color=orange>{log}</color>");
        return currentRequest;
    }
    public void EndRequest(string log = null)
    {
        OnLoadingEnd?.Invoke();
        OnEndRequest?.Invoke();

        if (_isLoadingPopupOpen)
        {
            _isLoadingPopupOpen = false;
            SceneManager.UnloadSceneAsync(_loadingPopup);
        }

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
        LogError(error);
    }
    public void OnRequestError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
        OnError?.Invoke(error.GenerateErrorReport());
        EndRequest();
    }
    public void LogError(string error)
    {
        if (_errorLoaded) return;
        _errorMessage = error;
        SceneManager.LoadSceneAsync(_errorPopupScene, LoadSceneMode.Additive);
    }
    #endregion

    public void DeleteAccountData()
    {
        _data = new();
        Mod<Mod_Economy>().UpdateCatPrices();
        StartCoroutine(Mod<Mod_Account>().UpdateData());
    }
}