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

    public void Init()
    {
        _meat = 10000; // TODO -> get from database
        _amountOfPurchases = new();
        _catPrices = new();

        for (int i = 0 ; i < GameManager.Instance.Cats.Length; i++)
        {
            _amountOfPurchases.Add(1); // TODO -> use database
            _catPrices.Add(GameManager.Instance.Cats[i].BasePrice); // TODO -> set base price with database
        }
    }

    public bool CanAdopt(int level)
    {
        if (_meat < _catPrices[level - 1])
        {
            Debug.Log($" You can't adopt this cat not enough money!");
            return false;
        }

        RemoveMeat(_catPrices[level - 1]);
        IncreasePrice(level);

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

    private void IncreasePrice(int level)
    {
        _amountOfPurchases[level - 1]++;
        // Calculation and update of new cat price (5% increase over current price)
        _catPrices[level - 1] = _catPrices[level - 1] + (_catPrices[level - 1] / 100 * 5);
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
                cheapestPrice = _catPrices[i];
                cheapestIndex = i;
            }
        }

        return cheapestIndex;
    }

}