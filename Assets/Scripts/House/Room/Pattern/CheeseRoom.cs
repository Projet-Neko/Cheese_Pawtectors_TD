public class CheeseRoom : Room
{
    private void Awake()
    {
        _security = RoomSecurity.Moved;
    }

    private void Start()
    {
        GameManager.Instance.Cheese.transform.position = transform.position;
    }
}