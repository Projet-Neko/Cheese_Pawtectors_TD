using System;

public class StartRoom : Room
{
    public static event Action<Room> OnInit;

    public override string DefineDesign()
    {
        return "";
    }

    protected override void Awake()
    {
        base.Awake();
        _security = RoomSecurity.Protected;
        _pattern = RoomPattern.StartRoom;
        OnInit?.Invoke(this);
    }
}