using System;

public class StartRoom : Room
{
    public static event Action<Room> OnInit;

    protected override void Awake()
    {
        base.Awake();
        _security = RoomSecurity.Protected;
        OnInit?.Invoke(this);
    }
}