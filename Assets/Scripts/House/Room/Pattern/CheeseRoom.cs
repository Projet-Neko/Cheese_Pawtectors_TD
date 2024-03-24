public class CheeseRoom : Room
{
    void Awake()
    {
        _security = RoomSecurity.Moved;
    }

    protected override bool CheckPath(Junction junction)
    {
        return true;
    }
}