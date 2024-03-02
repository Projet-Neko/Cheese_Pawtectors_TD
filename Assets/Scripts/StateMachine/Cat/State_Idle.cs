public class State_Idle : State
{
    public override void OnUpdate()
    {
        base.OnUpdate();

        Cat c = _brain.Entity as Cat;
        if (c.IsInStorageMode) return;

        // TODO -> Add idle movements

        if (!IsInFollowRange()) return;
        _brain.ChangeState(_brain.Follow);
        return;
    }
}