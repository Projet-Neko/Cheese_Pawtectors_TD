
using UnityEngine;

public class CheeseRoom : Room
{
    void Awake()
    {
        _security = RoomSecurity.Moved;
    }
}