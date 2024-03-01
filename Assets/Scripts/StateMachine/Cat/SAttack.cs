public class SAttack : State
{
    private Timer t;
    private Mouse m;

    public override void OnEnter(Brain brain)
    {
        base.OnEnter(brain);

        // Cat attack cooldown timer
        t = new(_brain.Entity.DPS);

        // Mouse script from target
        m = _brain.Target.GetComponentInParent<Mouse>();
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

        if (!t.IsRunning() || t.HasEnded())
        {
            Attack();
            t.Start();
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
        m.TakeDamage(_brain.Entity);
    }

    private void ResetTarget(Entity source)
    {
        if (source == m) _brain.Target = null;

        if (!_brain.Entity.IsAlive())
        {
            _brain.ChangeState(_brain.Sleep);
            return;
        }

        _brain.ChangeState(_brain.Idle);
    }
}