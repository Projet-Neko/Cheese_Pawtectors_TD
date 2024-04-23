public class CorridorRoom : Room
{
    public override string DefineDesign()
    {
        return _pathSO + "I_" + GetRoomDesign();
    }

    protected override void Awake()
    {
        base.Awake();
        _security = RoomSecurity.MovedAndRemoved;
        _pattern = RoomPattern.CorridorRoom;
    }
}