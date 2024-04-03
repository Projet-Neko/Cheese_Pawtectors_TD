public class VoidRoom : Room
{
    void Awake()
    {
        _security = RoomSecurity.Overwritten;
    }
}