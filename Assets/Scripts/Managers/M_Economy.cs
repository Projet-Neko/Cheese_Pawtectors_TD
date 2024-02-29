using UnityEngine;

public class M_Economy : MonoBehaviour
{
    public int Meat => _meat;

    private int _meat;

    public void Init()
    {
        _meat = 1000; // TODO -> get from database
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
}