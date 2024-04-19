public class CheeseRoom : Room
{
    protected override void Awake()
    {
        base.Awake();
        _security = RoomSecurity.Moved;
        GameManager.Instance.SpawnCheese(transform);
    }
}