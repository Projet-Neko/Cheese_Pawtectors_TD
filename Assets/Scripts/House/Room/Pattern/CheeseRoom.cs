public class CheeseRoom : Room
{
    public override string DefineDesign()
    {
        return "";
    }

    protected override void Awake()
    {
        base.Awake();
        _security = RoomSecurity.Moved;
        _pattern = RoomPattern.CheeseRoom;
        GameManager.Instance.SpawnCheese(transform);
    }
}