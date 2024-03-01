public class SWalk : State
{
    public override void OnUpdate()
    {
        base.OnUpdate();
        FollowTarget();
    }
}