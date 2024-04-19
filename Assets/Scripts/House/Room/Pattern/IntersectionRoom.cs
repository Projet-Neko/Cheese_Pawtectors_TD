public class IntersectionRoom : Room
{
    protected override void Awake()
    {
        base.Awake();
        _security = RoomSecurity.MovedAndRemoved;
    }
}