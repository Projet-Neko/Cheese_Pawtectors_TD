public class VoidRoom : Room
{
    void Awake()
    {
        _security = RoomSecurity.Overwritten;

        _openings[0] = false;
        _openings[1] = false;
        _openings[2] = false;
        _openings[3] = false;
    }
}