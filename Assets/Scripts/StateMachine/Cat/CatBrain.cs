public class CatBrain : Brain
{
    private void Awake()
    {
        ChangeState(Idle);
        _attackRange = transform.localScale.x * 8;
        _followRange = (transform.localScale.x * 4) * 8;
    }
}