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

    public List<int> AmountOfPurchases = new();
    public List<Data_Storage> Storage = new();
    public List<Data_Rooms> Rooms = new();
    public string LastUpdateString; // Needed for serialization

    private DateTime _lastUpdate;

    // Local save
    private byte[] _savedKey;
    private FileStream _dataStream;
    private readonly string _localDataKey = "LocalDataKey";

    public Data()
    {
        for (int i = 0; i < GameManager.Instance.Cats.Length; i++) AmountOfPurchases.Add(0);
        for (int i = 0; i < 8; i++) Storage.Add(new(i)); // Init empty storage
    }

    public byte[] Serialize() => Encoding.UTF8.GetBytes(JsonUtility.ToJson(this));

    public void UpdateLocalData(string json)
    {
        Data data = JsonUtility.FromJson<Data>(json);
        AmountOfPurchases = data.AmountOfPurchases;
        Storage = data.Storage;
        Rooms = data.Rooms;
        _lastUpdate = DateTime.Parse(data.LastUpdateString);

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
        Debug.Log(JsonUtility.ToJson(this));
        sWriter.Write(JsonUtility.ToJson(this));

        sWriter.Close();
        //iStream.Close();
        _dataStream.Close();
    }

    public void UpdateCatAmount(int level)
    {
        AmountOfPurchases[level]++;
        Update();
    }

    public void UpdateStorage(int slotIndex, int catIndex)
    {
        Storage[slotIndex].CatIndex = catIndex;
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