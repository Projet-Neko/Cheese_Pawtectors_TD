using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Internal;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Mod_Account : Mod
{
    public PlayFab.ClientModels.EntityKey Entity => _entity;
    public string PlayFabId => _playFabId;

    // PlayFab datas
    private PlayFab.ClientModels.EntityKey _entity;
    private string _playFabId;

    // Local save
    private AuthData _authData = new();
    private readonly BinaryFormatter _binaryFormatter = new();
    private string _path;

    // Login
    private bool _isFirstLogin;
    private bool _isLoggedIn = false;

    private string _username;

    public override void Init(GameManager gm)
    {
        base.Init(gm);
        Login();
    }

    #region Etape 1 : Check local save and login
    public void CheckLocalSave()
    {
        Debug.Log("Checking local datas...");

        _path = Application.persistentDataPath + "/CheesePawtectorsTD.save"; //Local save path
        Debug.Log($"Your save path is : {_path}");

        //Check if binary file with user datas exists
        if (!File.Exists(_path))
        {
            Debug.Log("No local datas found.");
            return;
        }

        Debug.Log("Local datas found !");

        try
        {
            using (FileStream file = new(_path, FileMode.Open))
                _authData = (AuthData)_binaryFormatter.Deserialize(file);

            if (_authData.Email == null) Debug.LogWarning("No registered account found.");
        }
        catch
        {
            Debug.LogError("Error with local datas.");
            File.Delete(_path);
        }
    }
    public void Login()
    {
        CheckLocalSave();

        if (_authData.Email == null)
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
        UserAccountInfo info = result.InfoResultPayload.AccountInfo;
        _isFirstLogin = result.LastLoginTime == null;

        //_gm.InvokeOnBigLoadingStart();

        //Use this line once to test PlayFab Register & Login
        //yield return RegisterAccount("testing@gmail.com", "testing");

        // --- Create Local Save
        using (FileStream file = new(_path, FileMode.Create)) _binaryFormatter.Serialize(file, _authData);

        //if (!_isFirstLogin && !IsAccountReset)
        if (!_isFirstLogin)
        {
            // --- Get Username ---

            // --- Get Account Data
            //GetAccountData();

            CompleteLogin();
            yield break;
        }

        Debug.LogWarning("First login !");

        // --- Create Username ---
        _username = "Kitten#";
        _username += SystemInfo.deviceUniqueIdentifier[..5];
        Debug.Log($"Creating username {_username}...");
        yield return (UpdateName(_username));

        // --- Create Data ---

        CompleteLogin();
        yield return null;
        //yield return UpdateData();
    }
    private void CompleteLogin()
    {
        _authData = new();
        _isLoggedIn = true;
        Debug.Log("Login complete !");
        _gm.InvokeOnLoginSuccess();
        //_gm.Testing();
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

    //public IEnumerator UpdateData()
    //{
    //    yield return _gm.StartAsyncRequest("Initiating data update...");

    //    PlayFabDataAPI.InitiateFileUploads(new()
    //    {
    //        Entity = new() { Id = _manager.Entity.Id, Type = _manager.Entity.Type },
    //        FileNames = new() { Data.GetType().Name }
    //    }, res =>
    //    {
    //        PlayFabHttp.SimplePutCall(res.UploadDetails[0].UploadUrl, Data.Serialize(), success =>
    //        {
    //            PlayFabDataAPI.FinalizeFileUploads(new()
    //            {
    //                Entity = new() { Id = _manager.Entity.Id, Type = _manager.Entity.Type },
    //                FileNames = new() { Data.GetType().Name }
    //            }, res => {
    //                _manager.EndRequest("Files uploaded !");

    //                Debug.Log($"init completed : {_isInitCompleted}");
    //                if (_isInitCompleted) return;
    //                _isInitCompleted = true;
    //                OnInitComplete?.Invoke();
    //            }, _manager.OnRequestError);
    //        }, error => Debug.LogError(error));
    //    }, _gm.OnRequestError);
    //}

    //public IEnumerator RegisterAccount(string email, string password)
    //{
    //    yield return _manager.StartAsyncRequest("Registering account...");
    //    CreateAccountData(email, password);
    //    string username = CreateUsername(email);

    //    PlayFabClientAPI.AddUsernamePassword(new()
    //    {
    //        Username = username, //Create unique username with email
    //        Email = email,
    //        Password = password //Password must be between 6 and 100 characters
    //    },
    //    res =>
    //    {
    //        PlayFabClientAPI.UnlinkCustomID(new()
    //        {
    //            CustomId = SystemInfo.deviceUniqueIdentifier
    //        }, res =>
    //        {
    //            _manager.EndRequest("Account registered !");
    //            StartCoroutine(UpdateName(username));
    //            CreateSave();
    //        }, _manager.OnRequestError);
    //    }, _manager.OnRequestError);
    //}

    //public void ResetAccount(bool admin = false)
    //{
    //    Debug.Log("Starting account reset...");
    //    IsAccountReset = true;

    //    if (admin) _authData = new();

    //    Login();
    //}
}