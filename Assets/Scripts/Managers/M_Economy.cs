using System.Collections.Generic;
using UnityEngine;

public class M_Economy : MonoBehaviour
{
    public int Meat => _meat;
    public List<int> CatPrices => _catPrices;

    private int _meat;
    private List<int> _amountOfPurchases; // Store the number of purchases for each cat
    private List<int> _catPrices;

    public void Init()
    {
        _meat = 1000; // TODO -> get from database
        _amountOfPurchases = new();
        _catPrices = new();

        for (int i = 0 ; i < GameManager.Instance.Cats.Length; i++)
        {
            _amountOfPurchases.Add(1); // TODO -> use database
            _catPrices.Add(GameManager.Instance.Cats[i].BasePrice); // TODO -> set base price with database
        }
    }

    public bool Adopt(int level)
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
        // _amountOfPurchases = index:levelduchat value: nbrd'achatduduchatdeceniveau
        _amountOfPurchases[level - 1]++;
        // Calculation and update of new cat price (5% increase over current price)
        _catPrices[level - 1] = _catPrices[level - 1] + (_catPrices[level - 1] / 100 * 5);
    }
}