public class IntersectionRoom : Room
{
    void Awake()
    {
        _security = RoomSecurity.MovedAndRemoved;
    }
}