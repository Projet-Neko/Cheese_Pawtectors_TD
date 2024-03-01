public class CatBrain : Brain
{
    private void Awake()
    {
        ChangeState(Idle);
        _attackRange = (transform.localScale.x / .6f) * 8;
        _followRange = (transform.localScale.x * 4) * 8;
    }
}