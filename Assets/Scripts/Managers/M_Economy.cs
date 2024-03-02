using System.Collections.Generic;
using UnityEngine;

public class M_Economy : MonoBehaviour
{
    public int Meat => _meat;
    public List<int> CatPrices => _catPrices;

    private int _meat;
    // Store the number of purchases for each cat index: catLevel, value: nbOfPurchasesOfCatsOfThisLevel
    private List<int> _amountOfPurchases; 
    private List<int> _catPrices;

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

    public void Init()
    {
        _meat = 10000; // TODO -> get from database
        _amountOfPurchases = new();
        _catPrices = new();

        for (int i = 0 ; i < GameManager.Instance.Cats.Length; i++)
        {
            _amountOfPurchases.Add(1); // TODO -> use database
            _catPrices.Add(GameManager.Instance.Cats[i].Level * 100); // TODO -> set base price with database
        }
    }

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

    public void AddMeat(int amount)
    {
        _meat += amount;
        Debug.Log($"Add {amount} Meat ! Current meat = {_meat}");
    }

    public void RemoveMeat(int amount)
    {
        _meat -= amount;
        Debug.Log($"Remove {amount} Meat ! Current meat = {_meat}");
    }

    private void IncreasePrice(int catLevel) 
    {
        _amountOfPurchases[catLevel]++;
        // Calculation and update of new cat price (5% increase over current price)
        _catPrices[catLevel] = _catPrices[catLevel] + (_catPrices[catLevel] / 100 * 5);
    }

    public int GetCheapestCatIndex()
    {
        // Temp variable 
        int cheapestIndex = 0;
        int cheapestPrice = _catPrices[cheapestIndex];

        // Browse prices for each cat
        for (int i = 1; i < _catPrices.Count; i++)
        {
            // Check whether the current price is lower than the price of the cheapest cat found so far
            if (_catPrices[i] < cheapestPrice)
            {
                // Update the price of the cheapest cat and its index
                cheapestIndex = i;
                cheapestPrice = _catPrices[cheapestIndex];
            }
        }
        return cheapestIndex;
    }
}