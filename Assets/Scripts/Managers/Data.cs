using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

[Serializable]
public class Data
{
    public DateTime LastUpdate => _lastUpdate;

    // Entities
    public List<bool> CatsUnlocked = new();
    public List<Data_Storage> Storage = new();
    public int LastCatUnlockedIndex;
    //public Cheese Cheese = new();

    // Waves
    public int WaveNumber = 1;
    public int MouseLevel = 1;

    // Economy
    public List<int> AmountOfPurchases = new();

    // Rooms
    public List<Data_Rooms> Rooms = new();

    // Social
    public int Clan = -1;
    public List<Data_Leaderboards> Leaderboards = new();
    public int Score => Leaderboards[0].Value;

    // Local save
    private byte[] _savedKey;
    private FileStream _dataStream;
    private readonly string _localDataKey = "LocalDataKey";

    // Update check
    public string LastUpdateString; // Needed for serialization
    public bool CloudNeedsUpdate;

    private DateTime _lastUpdate;

    public Data()
    {
        if (GameManager.Instance == null) return;

        // Init default amount of purchases and cats unlocked
        for (int i = 0; i < GameManager.Instance.Cats.Length; i++)
        {
            AmountOfPurchases.Add(0);
            CatsUnlocked.Add(GameManager.Instance.Cats[i].State == CatState.Unlock);
            LastCatUnlockedIndex = 0;
        }

        for (int i = 0; i < 8; i++) Storage.Add(new(i)); // Init empty storage

        // Init empty leaderboards
        foreach (var leaderboard in Enum.GetNames(typeof(Leaderboards)))
        {
            Leaderboards.Add(new()
            {
                Name = leaderboard,
                Value = 0
            });
        }
    }

    public byte[] Serialize() => Encoding.UTF8.GetBytes(JsonUtility.ToJson(this));

    public void UpdateLocalData(string json)
    {
        Data data = JsonUtility.FromJson<Data>(json);
        MouseLevel = data.MouseLevel;
        WaveNumber = data.WaveNumber;
        AmountOfPurchases = data.AmountOfPurchases;
        CatsUnlocked = data.CatsUnlocked;
        Storage = data.Storage;
        Rooms = data.Rooms;

        // Social
        Clan = data.Clan;
        Leaderboards = data.Leaderboards;

        _lastUpdate = DateTime.Parse(data.LastUpdateString);

        for (int i = 0; i < GameManager.Instance.Cats.Length; i++)
        {
            GameManager.Instance.Cats[i].State = CatsUnlocked[i] ? CatState.Unlock : CatState.Lock;
            if (CatsUnlocked[i]) LastCatUnlockedIndex = i;
        }

        Debug.Log(json);
    }

    public void Update()
    {
        _lastUpdate = DateTime.Now;
        LastUpdateString = _lastUpdate.ToString();

        string path = Application.persistentDataPath + "/CheesePawtectorsTD_LocalData.save"; //Local save path

        _dataStream = new(path, FileMode.Create);

        // Create encrypt keys
        Aes aes = Aes.Create();
        _savedKey = aes.Key;
        PlayerPrefs.SetString(_localDataKey, Convert.ToBase64String(_savedKey));
        byte[] inputIV = aes.IV;
        _dataStream.Write(inputIV, 0, inputIV.Length);

        // Write encrypted datas
        CryptoStream iStream = new(_dataStream, aes.CreateEncryptor(aes.Key, aes.IV), CryptoStreamMode.Write);
        StreamWriter sWriter = new(new CryptoStream(_dataStream, aes.CreateEncryptor(aes.Key, aes.IV), CryptoStreamMode.Write));
        //sWriter.Write(Serialize());
        //Debug.Log($"<color=cyan>Local data update : {JsonUtility.ToJson(this)}</color>");
        sWriter.Write(JsonUtility.ToJson(this));

        sWriter.Close();
        //iStream.Close();
        _dataStream.Close();
    }

    public void AdoptCat(int catIndex, int slotIndex, bool free)
    {
        Storage[slotIndex].CatIndex = catIndex;
        if (!free) AmountOfPurchases[catIndex]++;
        Update();
    }

    public void UnlockCat(int catIndex)
    {
        CatsUnlocked[catIndex] = true;
        Update();
    }

    public void UpdateWaves()
    {
        MouseLevel++;
        WaveNumber++;
        Update();
    }

    public void UpdateStorage(int slotIndex, int catIndex)
    {
        Storage[slotIndex].CatIndex = catIndex;
        Update();
    }

    public void UpdateClan(Clan clan)
    {
        Clan = (int)clan;
        Update();
    }

    public bool IsLocalDataMoreRecent(string json)
    {
        Debug.Log("Checking if local data is more recent...");
        GetLocalData();
        Debug.Log($"Local data last update : {_lastUpdate}");
        if (_lastUpdate == null) return false;

        Data data = JsonUtility.FromJson<Data>(json);
        DateTime lastCloudUpdate = DateTime.Parse(data.LastUpdateString);
        Debug.Log($"Cloud data last update : {lastCloudUpdate}");

        bool isLocalDataMoreRecent = lastCloudUpdate.CompareTo(_lastUpdate) < 0;
        string log = isLocalDataMoreRecent ? "Local data is more recent." : "Cloud data is more recent.";
        Debug.Log(log);

        CloudNeedsUpdate = isLocalDataMoreRecent;
        return isLocalDataMoreRecent;
    }

    private void GetLocalData()
    {
        string path = Application.persistentDataPath + "/CheesePawtectorsTD_LocalData.save"; //Local save path
        if (!File.Exists(path) || !PlayerPrefs.HasKey(_localDataKey)) return;

        // Get encrypt keys
        _savedKey = Convert.FromBase64String(PlayerPrefs.GetString(_localDataKey));
        _dataStream = new FileStream(path, FileMode.Open);
        Aes aes = Aes.Create();
        byte[] outputIV = new byte[aes.IV.Length];
        _dataStream.Read(outputIV, 0, outputIV.Length);

        // Get encrypted datas
        CryptoStream oStream = new(_dataStream, aes.CreateDecryptor(_savedKey, outputIV), CryptoStreamMode.Read);
        StreamReader reader = new(oStream);
        string text = reader.ReadToEnd();
        reader.Close();

        UpdateLocalData(text);
    }
}