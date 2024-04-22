public class TurnRoom : Room
{
    protected override void Awake()
    {
        base.Awake();
        _security = RoomSecurity.MovedAndRemoved;
        _pattern = RoomPattern.TurnRoom;
    }
}