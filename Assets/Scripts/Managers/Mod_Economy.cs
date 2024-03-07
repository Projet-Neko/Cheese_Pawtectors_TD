using PlayFab;
using PlayFab.EconomyModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Currency
{
    Meat, Pawsie, Meowstone
}

public class Mod_Economy : Mod
{
    public static event Action OnInitComplete;

    public List<int> CatPrices => _catPrices;

    // Local currencies
    public Dictionary<Currency, int> Currencies => _currencies;
    private readonly Dictionary<Currency, int> _currencies = new();

    // Store the number of purchases for each cat index: catLevel, value: nbOfPurchasesOfCatsOfThisLevel
    private List<int> _amountOfPurchases; // TODO -> add to database
    private List<int> _catPrices;

    // PlayFab Catalog
    private readonly List<CatalogItem> _catalogItems = new();
    private readonly Dictionary<string, Currency> _currenciesNameById = new();
    private readonly Dictionary<Currency, string> _currenciesIdByName = new();

    private bool _isInitComplete = false;

    private Timer _timer = new(60);

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
        if (obj is Mouse) AddCurrency(Currency.Meat, obj.Level);
    }

    public override void Init(GameManager gm)
    {
        base.Init(gm);
        _amountOfPurchases = new();
        _catPrices = new();

        for (int i = 0; i < GameManager.Instance.Cats.Length; i++)
        {
            _amountOfPurchases.Add(1); // TODO -> use database

            int n = GameManager.Instance.Cats[i].Level;
            _catPrices.Add(100 * (n - 1) + (100 * (int)Mathf.Pow(1.244415f, n - 1)));
            // TODO -> init with amount of purchases
        }

        StartCoroutine(EconomyDataRequest());
    }

    #region Cat Adoption
    public bool CanAdopt(int catLevel) 
    {
        if (_currencies[Currency.Meat] < _catPrices[catLevel])
        {
            Debug.Log($" You can't adopt this cat not enough money!");
            return false;
        }

        RemoveCurrency(Currency.Meat, _catPrices[catLevel]);
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
    public void AddCurrency(Currency currency, int amount)
    {
        _currencies[currency] += amount;
        Debug.Log($"Added {amount} {currency} ! Current {currency} = {_currencies[currency]}");
    }
    public void RemoveCurrency(Currency currency, int amount)
    {
        _currencies[currency] -= amount;
        Debug.Log($"Removed {amount} {currency} ! Current {currency} = {_currencies[currency]}");
    }
    public IEnumerator UpdateCurrency(Currency currency)
    {
        yield return _gm.StartAsyncRequest($"Update {currency}...");

        PlayFabEconomyAPI.UpdateInventoryItems(new()
        {
            Entity = new() { Id = _gm.Entity.Id, Type = _gm.Entity.Type },
            Item = new()
            {
                Amount = _currencies[currency],
                Id = _currenciesIdByName[currency]
            }
        }, res => _gm.EndRequest($"Updated {currency} !"), _gm.OnRequestError);
    }
    #endregion

    #region Economy Initialization
    public IEnumerator EconomyDataRequest()
    {
        yield return _gm.StartAsyncRequest("Getting game currencies and catalog items...");

        PlayFabEconomyAPI.SearchItems(new()
        {
            ContinuationToken = _gm.Token,
            Count = 50
        }, res =>
        {
            _catalogItems.AddRange(res.Items);
            _gm.EndRequest();

            // Relance la requête si tous les items n'ont pas été récupérés
            if (!string.IsNullOrEmpty(res.ContinuationToken))
            {
                _gm.Token = res.ContinuationToken;
                StartCoroutine(EconomyDataRequest());
                return;
            }

            InitEconomyData();
        }, _gm.OnRequestError);
    }
    private void InitEconomyData()
    {
        foreach (CatalogItem item in _catalogItems)
        {
            if (item.Type == "currency")
            {
                _currenciesNameById[item.Id] = Enum.Parse<Currency>(item.AlternateIds[0].Value);
                _currenciesIdByName[_currenciesNameById[item.Id]] = item.Id;
                _currencies[_currenciesNameById[item.Id]] = 0;
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

        PlayFabClientAPI.GetUserInventory(new(), res => GetPlayerInventory(), _gm.OnRequestError);
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

                if (item.Type == "currency") Currencies[_currenciesNameById[item.Id]] = (int)item.Amount;
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

            StartCoroutine(CheckOfflineCurrency());
        }, _gm.OnRequestError);
    }

    private IEnumerator CheckOfflineCurrency()
    {
        if (GameManager.Instance.LastLogin != null)
        {
            int meatGained = MeatGainedOffline(GameManager.Instance.LastLogin);
            Debug.Log($"Gained {meatGained} meat offline !");
            AddCurrency(Currency.Meat, meatGained);
            yield return UpdateCurrency(Currency.Meat);
        }
        
        OnInitComplete?.Invoke();
        DebugOnly();
    }
    #endregion

    int MeatPerSecond()
    {
        int mouseHealth = 4 + GameManager.Instance.MouseLevel;

        // TODO -> update speed variable with levels
        float catDPS = GameManager.Instance.GetLastUnlockedCatLevel() / 2.5f;
        float secondsToKill = mouseHealth / catDPS;

        float shootRate = 1 / secondsToKill;

        int meatGained = GameManager.Instance.MouseLevel / GameManager.Instance.SpawnTime;

        return (int)(meatGained / shootRate);
    }

    int MeatGainedOffline(DateTime? timeOffline)
    {
        TimeSpan ts = DateTime.UtcNow.Subtract(timeOffline.Value);
        int seconds = (int)ts.TotalSeconds;
        Debug.Log($"{seconds}s since last login.");

        int timeClamped = Mathf.Clamp(seconds, 0, 7200);

        return MeatPerSecond() * timeClamped;
    }


    private void DebugOnly()
    {
        Debug.Log("--- Economy Debug Function ---");
        _currencies[Currency.Meat] = 10000;
    }
}