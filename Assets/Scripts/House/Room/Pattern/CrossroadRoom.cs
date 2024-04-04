public class CrossroadRoom : Room
{
    void Awake()
    {
        _security = RoomSecurity.MovedAndRemoved;
    }
}