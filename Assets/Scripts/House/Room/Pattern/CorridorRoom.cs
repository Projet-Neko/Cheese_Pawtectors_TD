public class CorridorRoom : Room
{
    void Awake()
    {
        _security = RoomSecurity.MovedAndRemoved;
    }
}