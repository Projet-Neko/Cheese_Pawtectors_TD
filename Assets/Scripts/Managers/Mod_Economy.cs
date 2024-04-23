using PlayFab;
using PlayFab.EconomyModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Currency
{
    Treats, Pawsie, Meowcoin
}

public class Mod_Economy : Module
{
    [SerializeField] private GameObject _currencyPrefab;

    public static event Action<bool, int> OnAdoptCheck;
    public static event Action<int> treatWin; //Evet for the treatSuccess 


    public List<int> CatPrices => _catPrices;
    private List<int> _catPrices;

    // PlayFab Catalog
    private readonly List<CatalogItem> _catalogItems = new();
    private readonly Dictionary<string, Currency> _currenciesNameById = new();
    private readonly Dictionary<Currency, string> _currenciesIdByName = new();

    #region Gestion des évents
    private void Awake()
    {
        Entity.OnDeath += Entity_OnDeath;
        Storage.OnStorageCheck += CheckStorage_OnStorageCheck;
        CollectOfflineCurrency.OnCollect += CheckOfflineCurrency;
    }

    private void OnDestroy()
    {
        Entity.OnDeath -= Entity_OnDeath;
        Storage.OnStorageCheck -= CheckStorage_OnStorageCheck;
        CollectOfflineCurrency.OnCollect -= CheckOfflineCurrency;
    }
    private void Entity_OnDeath(Entity obj, bool hasBeenKilledByPlayer)
    {
        int currencyToAdd = obj.Level;
        //Debug.Log($"Cat current meatToAdd(Base) : {meatToAdd}");
        if (GameManager.Instance.IsPowerUpActive(PowerUpType.DoubleMeat))
        {
            currencyToAdd *= 2;
            //Debug.Log($"Cat current meatToAdd(DoubleMeat) : {meatToAdd}");
        }
        if (obj is Mouse && hasBeenKilledByPlayer) StartCoroutine(DisplayWinCurrency(currencyToAdd));
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
                Debug.Log($"Found currency {item.AlternateIds[0].Value} !");
                _currenciesNameById[item.Id] = Enum.Parse<Currency>(item.AlternateIds[0].Value);
                _currenciesIdByName[_currenciesNameById[item.Id]] = item.Id;
                _gm.Data.Currencies.Add(new(_currenciesNameById[item.Id]));
            }
        }

        if (_gm.LastLogin == null) // First time player
        {
            CompleteEconomyInit();
            return;
        }

        GetPlayerInventory();
    }
    #endregion

    #region Etape 2 : On récupère l'inventaire
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
                    if (item.Type != "currency") continue;
                    _gm.Data.Currencies[(int)_currenciesNameById[item.Id]].Amount = (int)item.Amount;
                }

                CompleteEconomyInit();
            }, _gm.OnRequestError);
        }, _gm.OnRequestError);
    }
    #endregion

    #region Currency Offline
    private void CheckOfflineCurrency()
    {
        int meatGained = MeatGainedOffline();
        Debug.Log($"<color=lime>Gained {meatGained} meat offline !</color>");
        AddCurrency(Currency.Treats, meatGained);
    }
    public int MeatPerSecond()
    {
        int mouseHealth = 4 + _gm.Data.MouseLevel;

        float catDPS = _gm.Cats[_gm.Data.LastCatUnlockedIndex].DPS();
        float secondsToKill = mouseHealth / catDPS;

        float shootRate = 1 / secondsToKill;

        int meatGained = _gm.Data.MouseLevel / _gm.SpawnTime;

        return (int)(meatGained / shootRate);
    }
    public int MeatGainedOffline()
    {
        TimeSpan ts = DateTime.UtcNow.Subtract(_gm.LastLogin.Value);
        int seconds = (int)ts.TotalSeconds;
        Debug.Log($"{seconds}s since last login.");

        int timeClamped = Mathf.Clamp(seconds, 0, 7200);

        return MeatPerSecond() * timeClamped;
    }
    #endregion

    private void CompleteEconomyInit()
    {
        UpdateCatPrices();
        InitComplete();
        DebugOnly();
    }

    public void UpdateCatPrices()
    {
        _catPrices = new();

        for (int i = 0; i < _gm.Cats.Length; i++)
        {
            int n = _gm.Cats[i].Level;
            int catPrice = 100 * (n - 1) + (100 * (int)Mathf.Pow(1.244415f, n - 1));
            _catPrices.Add(catPrice);

            for (int j = 0; j < _gm.Data.AmountOfPurchases[i]; j++) IncreasePrice(i);
            //Debug.Log($"{_gm.Cats[i].Name} price is {catPrice}. (bought {_gm.Data.AmountOfPurchases[i]} time)");
        }
    }

    #region Gestion de l'adoption
    private void CheckStorage_OnStorageCheck(int slotIndex, int catLevel)
    {
        if (slotIndex == -1) return;
        bool canAdopt;

        if (_gm.Data.Currencies[(int)Currency.Treats].Amount < _catPrices[catLevel - 1])
        {
            GameManager.Instance.LogError("You don't have enough treats !");
            canAdopt = false;
        }
        else
        {
            canAdopt = true;
            RemoveCurrency(Currency.Treats, _catPrices[catLevel - 1]);
            IncreasePrice(catLevel - 1);
        }

        OnAdoptCheck?.Invoke(canAdopt, catLevel);
    }
    private void IncreasePrice(int catIndex)
    {
        _catPrices[catIndex] = _catPrices[catIndex] + (_catPrices[catIndex] / 100 * 5);
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
        _gm.Data.Currencies[(int)currency].Amount += amount;
        //Debug.Log($"<color=lime>Added {amount} {currency} ! Current {currency} = {_gm.Data.Currencies[(int)currency].Amount}</color>");
        _gm.Data.Update();
        if (currency == Currency.Treats) treatWin?.Invoke(amount);
    }
    public void RemoveCurrency(Currency currency, int amount)
    {
        _gm.Data.Currencies[(int)currency].Amount -= amount;
        Debug.Log($"Removed {amount} {currency} ! Current {currency} = {_gm.Data.Currencies[(int)currency].Amount}");
        _gm.Data.Update();
    }
    public IEnumerator UpdateCurrency(Currency currency)
    {
        yield return _gm.StartAsyncRequest($"Update {currency}...");

        PlayFabEconomyAPI.UpdateInventoryItems(new()
        {
            Entity = new() { Id = _gm.Entity.Id, Type = _gm.Entity.Type },
            Item = new()
            {
                Amount = _gm.Data.Currencies[(int)currency].Amount,
                Id = _currenciesIdByName[currency]
            }
        }, res => _gm.EndRequest($"Updated {currency} !"), _gm.OnRequestError);
    }

    private IEnumerator DisplayWinCurrency(int currencyToAdd)
    {
        for (int i = 0; i < currencyToAdd; i++)
        {
            //Instantiate(_currencyPrefab, transform du parent avec modif de position);

            yield return new WaitForSeconds(.05f);
            AddCurrency(Currency.Treats, 1);

            //_currenciesText.text = _gm.Data.Currencies[(int)Currency.Treats].Amount.ToString();
        }
    }

    #endregion

    protected override void DebugOnly()
    {
        base.DebugOnly();
        AddCurrency(Currency.Treats, 1000);
    }
}