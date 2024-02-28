public class SAttack : State
{
    private Timer t;
    private Mouse m;

    public override void OnEnter(CatBrain brain)
    {
        base.OnEnter(brain);

        // Cat attack cooldown timer
        t = new(_brain.Cat.DPS);

        // Mouse script from target
        m = _brain.Target.GetComponentInParent<Mouse>();
        Entity.OnDeath += ResetTarget;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

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
        m.TakeDamage(_brain.Cat);
    }

    private void ResetTarget(Entity source)
    {
        if (source == m) _brain.Target = null;

        if (_brain.Cat.IsSleeping)
        {
            _brain.ChangeState(_brain.Sleep);
            return;
        }

        _brain.ChangeState(_brain.Idle);
    }
}