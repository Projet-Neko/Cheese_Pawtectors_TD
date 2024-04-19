public class VoidRoom : Room
{
    protected override void Awake()
    {
        base.Awake();
        _security = RoomSecurity.Overwritten;
    }
}