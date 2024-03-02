public class SWalk : State
{
    private bool _hasAttacked = false;

    public override void OnUpdate()
    {
        base.OnUpdate();
        FollowTarget();

        if (!_hasAttacked && IsInAttackRange())
        {
            _hasAttacked = true;
            _brain.Target.GetComponent<Cheese>().TakeDamage(_brain.Entity);
            _brain.Entity.Die(null);
        }
    }
}