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

public class Mod_Account : Mod
{
    public static event Action OnInitComplete;

    public PlayFab.ClientModels.EntityKey Entity => _entity;
    public string PlayFabId => _playFabId;
    public DateTime? LastLogin => _lastLogin;
    public bool IsLoggedIn => _isLoggedIn;

    // PlayFab datas
    private PlayFab.ClientModels.EntityKey _entity;
    private string _playFabId;

    // Local save
    private AuthData _authData = new();
    private string _path;
    private byte[] _savedKey;
    private FileStream _dataStream;
    private readonly string _localAuthDataKey = "LocalAuthDataKey";

    // Login
    private bool _isFirstLogin;
    private bool _isLoggedIn = false;
    private string _username;

    private DateTime? _lastLogin;

    public override void Init(GameManager gm)
    {
        base.Init(gm);
        Login();
    }

    #region Etape 1 : Check local save and login
    public void CheckLocalSave()
    {
        Debug.Log("<color=orange>Checking auth local datas...</color>");

        _path = Application.persistentDataPath + "/CheesePawtectorsTD_LocalAuthData.save"; //Local save path
        Debug.Log($"Your save path is : {_path}");

        //Check if binary file with user datas exists
        if (!File.Exists(_path) || !PlayerPrefs.HasKey(_localAuthDataKey))
        {
            Debug.Log("No auth local datas found.");
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
            Debug.LogError("Error with auth local datas.");
            File.Delete(_path);
        }
    }
    public void Login()
    {
        CheckLocalSave();

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
        _username = "Kitten#";
        yield return (UpdateName(_username));

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
        OnInitComplete?.Invoke();
        //_gm.InvokeOnLoginSuccess();
    }
    public IEnumerator UpdateName(string name)
    {
        yield return _gm.StartAsyncRequest("Updating username...");

        PlayFabClientAPI.UpdateUserTitleDisplayName(new()
        {
            DisplayName = name
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
                }, _gm.OnRequestError);
            }, error => Debug.LogError(error));
        }, _gm.OnRequestError);
    }

    private IEnumerator UpdateCloudData()
    {
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