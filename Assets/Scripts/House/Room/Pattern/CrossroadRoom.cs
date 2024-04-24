public class CrossroadRoom : Room
{
    public override string DefineDesign()
    {
        return _pathSO + "X_" + GetRoomDesign();
    }

    protected override void Awake()
    {
        base.Awake();
        _security = RoomSecurity.MovedAndRemoved;
        _pattern = RoomPattern.CrossraodRoom;
    }
}