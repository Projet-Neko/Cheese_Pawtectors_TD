using UnityEngine;

public class State_Walk : State
{
    private bool _hasAttacked = false;

    private bool TargetReached()
    {
        Debug.Log($"Mouse target : {_brain.Target.transform.position} & Mouse position : {_brain.Target.transform.position}");
        Debug.Log($"Mouse target : {_brain.Entity.transform.position == _brain.Target.transform.position}");
        return _brain.Entity.transform.position == _brain.Target.transform.position;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        FollowTarget();

        if (TargetReached())
        {
            Mouse mouse = (Mouse)_brain.Entity;
            _brain.Target = mouse.DefineTarget(_brain.Target);
        }

        if (!_hasAttacked && IsInAttackRange())
        {
            _hasAttacked = true;
            Cheese cheese = _brain.Target.GetComponent<Cheese>();// TO DO : LA TARGET N'EST JAMAIS LE FROMAGE
            if (cheese != null) cheese.TakeDamage(_brain.Entity);
            _brain.Entity.Die(null);
        }
    }
}