public class State_Idle : State
{
    public override void OnUpdate()
    {
        base.OnUpdate();

        if ((_brain.Entity as Cat).IsInStorageMode) return;

        // TODO -> Add idle movements

        if (!IsInFollowRange()) return;
        _brain.ChangeState(_brain.Follow);
        return;
    }
}