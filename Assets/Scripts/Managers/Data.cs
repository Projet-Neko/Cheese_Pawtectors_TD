using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class Data
{
    public List<int> AmountOfPurchases;

    public void Init()
    {
        InitEconomy();
    }

    public byte[] Serialize()
    {
        return Encoding.UTF8.GetBytes(JsonUtility.ToJson(this));
    }

    public void UpdateLocalData(string json)
    {
        Data data = JsonUtility.FromJson<Data>(json);
        //Account = data.Account;
        //Player = data.Player;
        //Inventory = data.Inventory;
        //Settings = data.Settings;

        //Player.UpdateEquippedSupports();
        //Player.UpdatePlayerStats();

        //Debug.Log($"User has {Inventory.Supports.Count} support(s).");
        //Debug.LogWarning($"{Account.MissionsData.Count}");

        //HasMissingDatas = Account.GetMissionsData();
    }

    public void InitEconomy()
    {
        //
    }
}