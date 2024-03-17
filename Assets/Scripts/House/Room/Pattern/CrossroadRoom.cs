public class CrossroadRoom : Room
{
    void Awake()
    {
        _security = RoomSecurity.MovedAndRemoved;

        _openings[0] = true;
        _openings[1] = true;
        _openings[2] = true;
        _openings[3] = true;
    }
}