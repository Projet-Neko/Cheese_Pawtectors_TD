public class TurnRoom : Room
{
    public override string DefineDesign()
    {
        return _pathSO + "L_" + GetRoomDesign();
    }

    protected override void Awake()
    {
        base.Awake();
        _security = RoomSecurity.MovedAndRemoved;
        _pattern = RoomPattern.TurnRoom;
    }
}