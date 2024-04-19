public class State_Attack : State
{
    private Timer _timer;
    private Mouse _mouse;

    public override void OnEnter(Brain brain)
    {
        base.OnEnter(brain);

        _timer = new(_brain.Entity.DPS); // Cat attack cooldown timer
        _mouse = _brain.Target.GetComponentInParent<Mouse>();
        Entity.OnDeath += ResetTarget;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!IsInAttackRange())
        {
            _brain.ChangeState(_brain.Follow);
            return;
        }

        if (!_timer.IsRunning() || _timer.HasEnded())
        {
            Attack();
            _timer.Start();
            return;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        Entity.OnDeath -= ResetTarget;
    }

    private void Attack()
    {
        _mouse.TakeDamage(_brain.Entity);
    }

    private void ResetTarget(Entity source, bool hasBeenKilledByPlayer)
    {
        if (source == _mouse) _brain.Target = null;

        if (!_brain.Entity.IsAlive())
        {
            _brain.ChangeState(_brain.Sleep);
            return;
        }

        _brain.ChangeState(_brain.Idle);
    }
}