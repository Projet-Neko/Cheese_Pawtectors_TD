public class State_Sleep : State
{
    public override void OnEnter(Brain brain)
    {
        base.OnEnter(brain);
        M_Wave.OnWaveReload += M_Wave_OnWaveReload;
    }

    public override void OnExit()
    {
        base.OnExit();
        M_Wave.OnWaveReload -= M_Wave_OnWaveReload;
    }

    private void M_Wave_OnWaveReload()
    {
        (_brain.Entity as Cat).WakeUp();
        _brain.ChangeState(_brain.Idle);
    }
}