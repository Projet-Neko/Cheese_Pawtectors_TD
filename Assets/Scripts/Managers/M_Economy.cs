using UnityEngine;

public class M_Economy : MonoBehaviour
{
    public int Meat => _meat;

    private int _meat;

    public void Init()
    {
        _meat = 0; // TODO -> get from database
    }

    public void AddMeat(int amount)
    {
        //
        _meat += amount;
        Debug.Log($"Add Meat ! Amount = {amount}, Meat = {_meat}");
    }

    public void RemoveMeat(int amount)
    {
        //_meat -= amount;
    }
}