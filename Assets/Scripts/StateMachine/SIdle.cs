public class SIdle : State
{
    public override void OnUpdate()
    {
        base.OnUpdate();

        // TODO -> Add idle movements

        if (!IsInFollowRange()) return;
        _brain.ChangeState(_brain.Follow);
        return;
    }
}