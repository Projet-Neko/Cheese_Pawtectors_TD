using PlayFab;
using PlayFab.EconomyModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mod_Economy : Mod
{
    public readonly Dictionary<Currency, int> Currencies = new(); //Player's currencies

    public int Meat => _meat;
    public List<int> CatPrices => _catPrices;

    private int _meat;
    // Store the number of purchases for each cat index: catLevel, value: nbOfPurchasesOfCatsOfThisLevel
    private List<int> _amountOfPurchases; 
    private List<int> _catPrices;

    // PlayFab Catalog
    private readonly List<CatalogItem> _catalogItems = new();
    private readonly Dictionary<string, string> _currencies = new();

    private void Awake()
    {
        Entity.OnDeath += Entity_OnDeath;
    }

    private void OnDestroy()
    {
        Entity.OnDeath -= Entity_OnDeath;
    }

    private void Entity_OnDeath(Entity obj)
    {
        if (obj is Mouse) AddMeat(obj.Level);
    }

    public override void Init(GameManager gm)
    {
        base.Init(gm);
        _meat = 10000; // TODO -> get from database
        _amountOfPurchases = new();
        _catPrices = new();

        for (int i = 0; i < GameManager.Instance.Cats.Length; i++)
        {
            _amountOfPurchases.Add(1); // TODO -> use database

            int n = GameManager.Instance.Cats[i].Level;
            _catPrices.Add(100 * (n - 1) + (100 * (int)Mathf.Pow(1.244415f, n - 1)));
        }
    }

    #region Cat Adoption
    public bool CanAdopt(int catLevel) 
    {
        if (_meat < _catPrices[catLevel])
        {
            Debug.Log($" You can't adopt this cat not enough money!");
            return false;
        }

        RemoveMeat(_catPrices[catLevel]);
        IncreasePrice(catLevel);

        return true;
    }

    private void IncreasePrice(int catLevel)
    {
        _amountOfPurchases[catLevel]++;
        _catPrices[catLevel] = _catPrices[catLevel] + (_catPrices[catLevel] / 100 * 5);
    }

    public int GetCheapestCatIndex()
    {
        int cheapestIndex = 0;

        for (int i = 1; i < _catPrices.Count; i++)
        {
            // Check whether the current price is lower than the price of the cheapest cat found so far
            if (_catPrices[i] < _catPrices[cheapestIndex]) cheapestIndex = i;
        }

        return cheapestIndex;
    }
    #endregion

    #region Currencies
    public void AddMeat(int amount)
    {
        _meat += amount;
        Debug.Log($"Added {amount} Meat ! Current meat = {_meat}");
    }

    public void RemoveMeat(int amount)
    {
        _meat -= amount;
        Debug.Log($"Removed {amount} Meat ! Current meat = {_meat}");
    }

    //public IEnumerator AddCurrency(Currency currency, int amount)
    //{
    //    yield return _gm.StartAsyncRequest($"Adding {amount} {currency}...");

    //    PlayFabEconomyAPI.AddInventoryItems(new()
    //    {
    //        Entity = new() { Id = _gm.Entity.Id, Type = _gm.Entity.Type },
    //        Amount = amount,
    //        Item = new()
    //        {
    //            AlternateId = new()
    //            {
    //                Type = "FriendlyId",
    //                Value = currency.ToString()
    //            }
    //        }
    //    }, res =>
    //    {
    //        Currencies[currency] += amount;
    //        //_gm.InvokeOnCurrencyUpdate();
    //        //_gm.InvokeOnCurrencyGained(currency, amount);
    //        _gm.EndRequest($"Added {amount} {currency} !");
    //    }, _gm.OnRequestError);
    //}
    #endregion

    public void GetEconomyData()
    {
        StartCoroutine(EconomyDataRequest());
    }

    public IEnumerator EconomyDataRequest()
    {
        yield return _gm.StartAsyncRequest("Getting game currencies and catalog items...");

        PlayFabEconomyAPI.SearchItems(new()
        {
            ContinuationToken = _gm.Token,
            Count = 50
        }, res =>
        {
            //CatalogItems.AddRange(res.Items);
            _gm.EndRequest();

            // Relance la requête si tous les items n'ont pas été récupérés
            if (!string.IsNullOrEmpty(res.ContinuationToken))
            {
                _gm.Token = res.ContinuationToken;
                StartCoroutine(EconomyDataRequest());
                return;
            }

            //InitEconomyData();
        }, _gm.OnRequestError);
    }

    private void InitEconomyData()
    {
        foreach (CatalogItem item in _catalogItems)
        {
            //TODO -> Get bundle items and shops

            // Initialise la liste des currencies du joueur à 0
            if (item.Type == "currency")
            {
                _currencies[item.Id] = item.AlternateIds[0].Value;
                //CurrencyData data = JsonUtility.FromJson<CurrencyData>(item.DisplayProperties.ToString());
                Currencies[Enum.Parse<Currency>(_currencies[item.Id])] = 0;
            }
            //else if (item.Type == "catalogItem" || item.Type == "bundle")
            //{
            //    _itemsById[item.Id] = item.AlternateIds[0].Value;
            //    _itemsByName[item.AlternateIds[0].Value] = item.Id;
            //}
            //else if (item.Type == "store")
            //{
            //    _storesById[item.Id] = item.AlternateIds[0].Value;
            //    _storesByName[item.AlternateIds[0].Value] = item.Id;
            //    Stores[item.Id] = item;
            //}
        }

        //if (_manager.IsFirstLogin)
        //{
        //    StartCoroutine(CreateInitialCurrencies());
        //    return;
        //}

        PlayFabClientAPI.GetUserInventory(new(), res =>
        {
            //Energy = res.VirtualCurrency["EN"];
            GetPlayerInventory();
        }, _gm.OnRequestError);
    }

    private void GetPlayerInventory()
    {
        _gm.StartRequest("Getting player's inventory...");

        PlayFabEconomyAPI.GetInventoryItems(new()
        {
            Entity = new() { Id = _gm.Entity.Id, Type = _gm.Entity.Type }
        }, res =>
        {
            _gm.EndRequest();

            foreach (InventoryItem item in res.Items)
            {
                //if (_gm.IsAccountReset)
                //{
                //    StartCoroutine(DeleteItem(item));
                //    continue;
                //}

                if (item.Type == "currency")
                {
                    Currencies[Enum.Parse<Currency>(_currencies[item.Id])] = (int)item.Amount;
                }
                //else if (item.Type == "catalogItem")
                //{
                //    Type type = Type.GetType(_itemsById[item.Id]);
                //    Activator.CreateInstance(type, item);
                //}
            }

            //if (_gm.IsAccountReset)
            //{
            //    StartCoroutine(CreateInitialCurrencies());
            //    return;
            //}

            //_gm.Data.UpdateEquippedGears();
            //OnInitComplete?.Invoke();
        }, _gm.OnRequestError);
    }
}