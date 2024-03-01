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
        _attackRange = (transform.localScale.x / 1.5f) * 8;
        _followRange = 0;
        ChangeState(Walk);
    }

    protected override void Update()
    {
        base.Update();

        if (_currentState is not SSleep && Entity.IsAttacked) ChangeState(Sleep);
    }
}