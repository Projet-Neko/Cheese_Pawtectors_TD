public class CorridorRoom : Room
{
    void Awake()
    {
        _security = RoomSecurity.MovedAndRemoved;

        _openings[0] = false;
        _openings[1] = true;
        _openings[2] = false;
        _openings[3] = true;
    }
}