public class VoidRoom : Room
{
    public override string DefineDesign()
    {
        return "";
    }

    protected override void Awake()
    {
        base.Awake();
        _security = RoomSecurity.Overwritten;
        _pattern = RoomPattern.VoidRoom;
    }
}