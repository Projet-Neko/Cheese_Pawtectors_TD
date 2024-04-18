public class CheeseRoom : Room
{
    private void Awake()
    {
        _security = RoomSecurity.Moved;
    }

    protected override void Start()
    {
        base.Start();
        GameManager.Instance.Cheese.transform.position = transform.position;
    }
}