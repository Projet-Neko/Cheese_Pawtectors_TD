using System;

public class StartRoom : Room
{
    public static event Action<Room> OnInit;

    void Awake()
    {
        _security = RoomSecurity.Protected;
        OnInit?.Invoke(this);
    }
}