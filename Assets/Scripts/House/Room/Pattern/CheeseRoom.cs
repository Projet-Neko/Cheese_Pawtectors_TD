public class CheeseRoom : Room
{
    private void Awake()
    {
        _security = RoomSecurity.Moved;
        GameManager.Instance.SpawnCheese(transform);
    }
}