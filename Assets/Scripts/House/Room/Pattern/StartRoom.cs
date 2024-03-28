using System.Collections;
using UnityEngine;

public class StartRoom : Room
{
    void Awake()
    {
        _security = RoomSecurity.Protected;
    }

    public bool CheckPath()
    {
        _correctPath = _opening[0].Validation();
        return _correctPath;
    }
}