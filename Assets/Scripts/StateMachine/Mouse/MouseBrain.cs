using UnityEngine;

public class MouseBrain : Brain
{
    private void Start()
    {
        Target = ((Mouse)Entity).DefineTarget(null);
        Debug.Log($"Mouse target : {Target.transform.position}");
        _attackRange = _collider.bounds.size.x / 2;
        _followRange = 0;
        ChangeState(Walk);
    }

    protected override void Update()
    {
        base.Update();

        if ((Entity as Mouse).IsBoss) return;
        if (_currentState is not State_Freeze && Entity.IsAttacked) ChangeState(Freeze);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}