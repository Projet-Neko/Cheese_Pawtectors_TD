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
    public Dictionary<Currency, int> Currencies => _currencies; // Local currencies

    private List<int> _catPrices;
    private readonly Dictionary<Currency, int> _currencies = new();

    // PlayFab Catalog
    private readonly List<CatalogItem> _catalogItems = new();
    private readonly Dictionary<string, Currency> _currenciesNameById = new();
    private readonly Dictionary<Currency, string> _currenciesIdByName = new();

    #region Gestion des évents
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
    #endregion

    public override void Init(GameManager gm)
    {
        base.Init(gm);
        StartCoroutine(EconomyDataRequest());
    }

    #region Etape 1 : On récupère les données du jeu
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

        if (_gm.LastLogin == null) // First time player
        {
            CompleteEconomyInit();
            return;
        }

        GetPlayerInventory();
    }
    #endregion

    #region Etape 2 : On récupère l'inventaire et la currency offline du joueur
    private void GetPlayerInventory()
    {
        _gm.StartRequest("Getting player's inventory...");

        PlayFabClientAPI.GetUserInventory(new(), res =>
        {
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
        }, _gm.OnRequestError);
    }
    private IEnumerator CheckOfflineCurrency()
    {
        int meatGained = MeatGainedOffline(GameManager.Instance.LastLogin);
        Debug.Log($"<color=lime>Gained {meatGained} meat offline !</color>");
        AddCurrency(Currency.Meat, meatGained);
        yield return UpdateCurrency(Currency.Meat);

        CompleteEconomyInit();
    }
    private int MeatPerSecond()
    {
        int mouseHealth = 4 + GameManager.Instance.MouseLevel;

        // TODO -> update speed variable with levels
        float catDPS = GameManager.Instance.GetLastUnlockedCatLevel() / 2.5f;
        float secondsToKill = mouseHealth / catDPS;

        float shootRate = 1 / secondsToKill;

        int meatGained = GameManager.Instance.MouseLevel / GameManager.Instance.SpawnTime;

        return (int)(meatGained / shootRate);
    }
    private int MeatGainedOffline(DateTime? timeOffline)
    {
        TimeSpan ts = DateTime.UtcNow.Subtract(timeOffline.Value);
        int seconds = (int)ts.TotalSeconds;
        Debug.Log($"{seconds}s since last login.");

        int timeClamped = Mathf.Clamp(seconds, 0, 7200);

        return MeatPerSecond() * timeClamped;
    }
    #endregion

    private void CompleteEconomyInit()
    {
        _catPrices = new();

        Debug.Log($"Setting cat prices... {GameManager.Instance.Cats.Length} cats found.");

        for (int i = 0; i < GameManager.Instance.Cats.Length; i++)
        {
            int n = GameManager.Instance.Cats[i].Level;
            int catPrice = 100 * (n - 1) + (100 * (int)Mathf.Pow(1.244415f, n - 1));

            if (_gm.Data.AmountOfPurchases[i] != 0)
            {
                catPrice = (catPrice / 100 * 5) * _gm.Data.AmountOfPurchases[i] - 1;
            }

            _catPrices.Add(catPrice);

            Debug.Log($"{GameManager.Instance.Cats[i].Name} price is {catPrice}. (bought {_gm.Data.AmountOfPurchases[i]} time)");
        }

        OnInitComplete?.Invoke();
        DebugOnly();
    }

    #region Gestion de l'adoption
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
        _gm.Data.UpdateCatAmount(catLevel);
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

    #region Gestion des currencies
    public void AddCurrency(Currency currency, int amount)
    {
        _currencies[currency] += amount;
        Debug.Log($"<color=lime>Added {amount} {currency} ! Current {currency} = {_currencies[currency]}</color>");
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
}