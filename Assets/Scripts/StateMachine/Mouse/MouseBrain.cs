public class MouseBrain : Brain
{
    private void Start()
    {
        Target = GameManager.Instance.Cheese.gameObject;
        _attackRange = _collider.bounds.size.x / 2;
        _followRange = 0;
        ChangeState(Walk);
    }

    protected override void Update()
    {
        base.Update();

        if (_currentState is not State_Sleep && Entity.IsAttacked) ChangeState(Sleep);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}