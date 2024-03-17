public class CheeseRoom : Room
{
    void Awake()
    {
        _security = RoomSecurity.Moved;

        _openings[0] = false;
        _openings[1] = false;
        _openings[2] = false;
        _openings[3] = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}