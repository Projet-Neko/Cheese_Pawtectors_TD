public class CatBrain : Brain
{
    private void Awake()
    {
        ChangeState(Idle);
        _attackRange = transform.localScale.x * 8;
        _followRange = (transform.localScale.x * 4) * 8;
    }

    protected override void Update()
    {
        base.Update();
        if (_currentState is not State_Sleep && !_entity.IsAlive()) ChangeState(Sleep);
    }
}