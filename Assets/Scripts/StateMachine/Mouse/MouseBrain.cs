public class MouseBrain : Brain
{
    private void Start()
    {
        _attackRange = (transform.localScale.x / 1.5f) * 8;
        _followRange = 0;
        Target = GameManager.Instance.Cheese.gameObject;
        ChangeState(Walk);
    }
}