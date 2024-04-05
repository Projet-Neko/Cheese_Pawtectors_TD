using System.Collections;
using UnityEngine;

public class StartRoom : Room
{
    void Awake()
    {
        _security = RoomSecurity.Protected;
    }
}