using PlayFab;
using PlayFab.ClientModels;
using PlayFab.DataModels;
using PlayFab.Internal;
using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class Mod_Account : Module
{
    public static event Action<bool> OnLocalDataCheck;
    public static event Action OnCloudUpdate;
    public static event Action OnLoginStart;

    // PlayFab datas
    public PlayFab.ClientModels.EntityKey Entity => _entity;
    public string PlayFabId => _playFabId;

    private PlayFab.ClientModels.EntityKey _entity;
    private string _playFabId;

    // Local save
    private AuthData _authData = new();
    private string _path;
    private byte[] _savedKey;
    private FileStream _dataStream;
    private readonly string _localAuthDataKey = "LocalAuthDataKey";

    // Login
    public DateTime? LastLogin => _lastLogin;
    public bool IsLoggedIn => _isLoggedIn;

    private bool _isFirstLogin;
    private bool _isLoggedIn = false;
    private DateTime? _lastLogin;

    // Username
    public string Username => HasChangedUsername ? _username.Split("#")[0] : _username;
    public bool HasChangedUsername => !_username.StartsWith(_defaultUsername);

    private string _username;
    private readonly string _defaultUsername = "Kitten#";

    public override void Init(GameManager gm)
    {
        base.Init(gm);
        CheckLocalSave();
    }

    #region Etape 1 : Check local save and login
    public void CheckLocalSave()
    {
        _gm.StartRequest("Checking auth local datas...");

        _path = Application.persistentDataPath + "/CheesePawtectorsTD_LocalAuthData.save"; //Local save path
        Debug.Log($"Your save path is : {_path}");

        //Check if binary file with user datas exists
        if (!File.Exists(_path) || !PlayerPrefs.HasKey(_localAuthDataKey))
        {
            _gm.EndRequest("No auth local datas found.");
            OnLocalDataCheck?.Invoke(false);
            return;
        }

        Debug.Log("<color=lime>Auth local datas found !</color>");

        try
        {
            // Get encrypt keys
            _savedKey = Convert.FromBase64String(PlayerPrefs.GetString(_localAuthDataKey));
            _dataStream = new FileStream(_path, FileMode.Open);
            Aes aes = Aes.Create();
            byte[] outputIV = new byte[aes.IV.Length];
            _dataStream.Read(outputIV, 0, outputIV.Length);

            // Get encrypted datas
            CryptoStream oStream = new(_dataStream, aes.CreateDecryptor(_savedKey, outputIV), CryptoStreamMode.Read);
            StreamReader reader = new(oStream);
            string text = reader.ReadToEnd();
            reader.Close();

            _authData = JsonUtility.FromJson<AuthData>(text);
            if (string.IsNullOrEmpty(_authData.Email)) Debug.LogWarning("No registered account found.");
        }
        catch
        {
            _gm.OnRequestError("Error with auth local datas.");
            File.Delete(_path);
            OnLocalDataCheck?.Invoke(false);
            return;
        }

        _gm.EndRequest();
        OnLocalDataCheck?.Invoke(true);
        Login();
    }
    public void Login()
    {
        OnLoginStart?.Invoke();

        if (string.IsNullOrEmpty(_authData.Email))
        {
            _gm.StartRequest("Starting anonymous login...");

            PlayFabClientAPI.LoginWithCustomID(new()
            {
                CustomId = SystemInfo.deviceUniqueIdentifier,
                CreateAccount = true,
                InfoRequestParameters = new() { GetUserAccountInfo = true }
            }, res => StartCoroutine(OnLoginRequestSuccess(res)), _gm.OnRequestError);
            return;
        }

        Login(_authData.Email, _authData.Password);
    }
    public void Login(string email, string password) // Called from login popup
    {
        OnLoginStart?.Invoke();
        _gm.StartRequest($"Starting login to {email}...");

        _authData = new()
        {
            Email = email,
            Password = password
        };

        PlayFabClientAPI.LoginWithEmailAddress(new()
        {
            Email = email,
            Password = password,
            InfoRequestParameters = new() { GetUserAccountInfo = true }
        }, res => StartCoroutine(OnLoginRequestSuccess(res)), OnLoginRequestError);
    }
    private void OnLoginRequestError(PlayFabError error)
    {
        //_gm.InvokeOnLoginError();
        _gm.OnRequestError(error);
        _authData = new();

        if (File.Exists(_path)) File.Delete(_path);
        Login();
    }
    #endregion

    #region Etape 2 : Create local datas on login success
    private IEnumerator OnLoginRequestSuccess(LoginResult result)
    {
        _gm.EndRequest("Login request success !");

        // --- Get PlayFab datas
        _playFabId = result.PlayFabId;
        _entity = result.EntityToken.Entity;

        // --- Check if first login
        _lastLogin = result.LastLoginTime;
        _isFirstLogin = _lastLogin == null;

        //Use this line once to test PlayFab Register & Login
        //yield return RegisterAccount("testing@gmail.com", "testing");

        UpdateLocalSave();

        //if (!_isFirstLogin && !IsAccountReset)
        if (!_isFirstLogin)
        {
            // --- Get Username ---
            UserAccountInfo info = result.InfoResultPayload.AccountInfo;
            _username = info.TitleInfo.DisplayName;

            // --- Get Account Data
            GetUserFiles();
            yield break;
        }

        Debug.LogWarning("First login !");

        // --- Create Username ---
        yield return (UpdateUsername(_defaultUsername));

        // --- Create Data ---
        // TODO

        CompleteLogin();
        yield return null;
        //yield return UpdateData();
    }
    private void UpdateLocalSave()
    {
        _dataStream = new FileStream(_path, FileMode.Create);

        // Create encrypt keys
        Aes aes = Aes.Create();
        _savedKey = aes.Key;
        PlayerPrefs.SetString(_localAuthDataKey, Convert.ToBase64String(_savedKey));
        byte[] inputIV = aes.IV;
        _dataStream.Write(inputIV, 0, inputIV.Length);

        // Write encrypted datas
        CryptoStream iStream = new(_dataStream, aes.CreateEncryptor(aes.Key, aes.IV), CryptoStreamMode.Write);
        StreamWriter sWriter = new(new CryptoStream(_dataStream, aes.CreateEncryptor(aes.Key, aes.IV), CryptoStreamMode.Write));
        sWriter.Write(JsonUtility.ToJson(_authData));

        sWriter.Close();
        //iStream.Close();
        _dataStream.Close();

        _authData = new();
    }
    private void CompleteLogin()
    {
        _isLoggedIn = true;
        Debug.Log("<color=lime>Login complete !</color>");
        InitComplete();
        //_gm.InvokeOnLoginSuccess();
    }
    public IEnumerator UpdateUsername(string username)
    {
        yield return _gm.StartAsyncRequest("Updating username...");
        _username = username;

        PlayFabClientAPI.UpdateUserTitleDisplayName(new()
        {
            DisplayName = username
        }, res => _gm.EndRequest(), _gm.OnRequestError);
    }
    #endregion

    #region Etape 3 : Get cloud datas if needed
    public void GetUserFiles()
    {
        _gm.StartRequest("Getting user files...");

        PlayFabDataAPI.GetFiles(new()
        {
            Entity = new() { Id = _gm.Entity.Id, Type = _gm.Entity.Type }
        }, res =>
        {
            _gm.EndRequest($"Obtained {res.Metadata.Count} file(s) !");
            GetFilesDatas(res.Metadata[_gm.Data.GetType().Name]);
        }, _gm.OnRequestError);
    }
    private void GetFilesDatas(GetFileMetadata file)
    {
        _gm.StartRequest();

        PlayFabHttp.SimpleGetCall(file.DownloadUrl, res =>
        {
            _gm.EndRequest();

            if (_gm.Data.IsLocalDataMoreRecent(Encoding.UTF8.GetString(res)))
            {
                StartCoroutine(UpdateCloudData());
                return;
            }

            _gm.Data.UpdateLocalData(Encoding.UTF8.GetString(res));
            CompleteLogin();
        }, error => Debug.LogError(error));
    }
    #endregion

    public IEnumerator RegisterAccount(string email, string password)
    {
        yield return _gm.StartAsyncRequest("Registering account...");

        PlayFabClientAPI.AddUsernamePassword(new()
        {
            Username = _username,
            Email = email,
            Password = password //Password must be between 6 and 100 characters
        },
        res =>
        {
            PlayFabClientAPI.UnlinkCustomID(new()
            {
                CustomId = SystemInfo.deviceUniqueIdentifier
            }, res =>
            {
                _gm.EndRequest("Account registered !");
                UpdateLocalSave();
            }, _gm.OnRequestError);
        }, _gm.OnRequestError);
    }

    //public IEnumerator DeleteAccount()
    //{
    //    yield return _gm.StartAsyncRequest("Deleting account...");

    //    PlayFabClientAPI.DeletePlayer(new()
    //    {
    //        CustomId = SystemInfo.deviceUniqueIdentifier
    //    }, res =>
    //    {
    //        _gm.EndRequest("Account registered !");
    //        SetLocalSave();
    //    }, _gm.OnRequestError);
    //}

    public IEnumerator UpdateData()
    {
        yield return _gm.StartAsyncRequest("Initiating data update...");
        _gm.Data.Update();

        PlayFabDataAPI.InitiateFileUploads(new()
        {
            Entity = new() { Id = _gm.Entity.Id, Type = _gm.Entity.Type },
            FileNames = new() { _gm.Data.GetType().Name }
        }, res =>
        {
            PlayFabHttp.SimplePutCall(res.UploadDetails[0].UploadUrl, _gm.Data.Serialize(), success =>
            {
                PlayFabDataAPI.FinalizeFileUploads(new()
                {
                    Entity = new() { Id = _gm.Entity.Id, Type = _gm.Entity.Type },
                    FileNames = new() { _gm.Data.GetType().Name }
                }, res =>
                {
                    _gm.EndRequest("Files uploaded !");
                    OnCloudUpdate?.Invoke();
                }, _gm.OnRequestError);
            }, error => Debug.LogError(error));
        }, _gm.OnRequestError);
    }

    private IEnumerator UpdateCloudData()
    {
        Debug.Log("Update cloud data...");
        yield return UpdateData();
        CompleteLogin();
    }

    //public void ResetAccount(bool admin = false)
    //{
    //    Debug.Log("Starting account reset...");
    //    IsAccountReset = true;

    //    if (admin) _authData = new();

    //    Login();
    //}
}