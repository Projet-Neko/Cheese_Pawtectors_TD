public class IntersectionRoom : Room
{
    public override string DefineDesign()
    {
        return _pathSO + "T_" + GetRoomDesign();
    }

    protected override void Awake()
    {
        base.Awake();
        _security = RoomSecurity.MovedAndRemoved;
        _pattern = RoomPattern.IntersectionRoom;
    }
}