
using UnityEngine;

public class CheeseRoom : Room
{
    void Awake()
    {
        _security = RoomSecurity.Moved;
    }

    protected override bool CheckPath(Junction junction)
    {
        Debug.Log("CheckPath Cheese Room");
        return true;
    }
}