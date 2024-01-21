public class Cheese : Entity
{
    private void Awake()
    {
        _baseHealth = _currentHealth = 30;
    }

    protected override void Death(Entity source)
    {
        // TODO -> event game over
    }
}