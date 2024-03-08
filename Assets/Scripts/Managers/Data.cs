using System;
using System.Collections.Generic;
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
    }

    public void InitEconomy()
    {
        AmountOfPurchases = new();

        for (int i = 0; i < GameManager.Instance.Cats.Length; i++)
        {
            AmountOfPurchases.Add(1);
        }

        // Init empty storage
        for (int i = 0; i < 8; i++) Storage.Add(new(i));
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
}