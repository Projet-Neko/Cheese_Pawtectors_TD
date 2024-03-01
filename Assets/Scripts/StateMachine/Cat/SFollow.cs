public class SFollow : State
{
    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!IsInFollowRange())
        {
            _brain.ChangeState(_brain.Idle);
            return;
        }

        if (IsInAttackRange()) _brain.ChangeState(_brain.Attack);
        FollowTarget();
    }
}