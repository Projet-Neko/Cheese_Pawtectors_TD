using UnityEngine;

public class SFollow : State
{
    public override void OnEnter(CatBrain brain)
    {
        base.OnEnter(brain);
        //_brain.Agent.SetDestination(_target.transform.position);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        //if (_brain.Agent.remainingDistance == 0) _brain.ChangeState(_brain.Attack);

        if (!IsInFollowRange())
        {
            _brain.ChangeState(_brain.Idle);
            return;
        }

        if (IsInAttackRange()) _brain.ChangeState(_brain.Attack);
    }
}