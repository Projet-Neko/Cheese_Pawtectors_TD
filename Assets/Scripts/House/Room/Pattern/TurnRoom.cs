public class TurnRoom : Room
{
    void Awake()
    {
        _security = RoomSecurity.MovedAndRemoved;
    }
}