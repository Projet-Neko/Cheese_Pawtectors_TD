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
    }

    public void RemoveMeat(int amount)
    {
        //
    }
}