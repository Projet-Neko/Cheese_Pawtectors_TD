public class StartRoom : Room
{
    void Awake()
    {
        _security = RoomSecurity.Protected;

        _openings[0] = false;
        _openings[1] = true;
        _openings[2] = false;
        _openings[3] = false;
    }
}