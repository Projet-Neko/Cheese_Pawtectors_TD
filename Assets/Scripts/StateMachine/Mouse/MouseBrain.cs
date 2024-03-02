public class MouseBrain : Brain
{
    private void Awake()
    {
        Cheese.OnInit += Cheese_OnInit;
    }

    private void OnDestroy()
    {
        Cheese.OnInit -= Cheese_OnInit;
    }

    private void Cheese_OnInit(Cheese obj)
    {
        Target = obj.gameObject;
    }

    private void Start()
    {
        _attackRange = _renderer.sprite.bounds.size.x/5;
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