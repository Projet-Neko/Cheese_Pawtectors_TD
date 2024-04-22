using System;
using UnityEngine;

public class MouseBrain : Brain
{
    public static event Func<Vector3, GameObject> VisitedNextRoom;// Get the next room visited by the mouse

    protected virtual void Start()
    {
        Target = VisitedNextRoom?.Invoke(transform.position);
        _attackRange = _collider.bounds.size.x / 2;
        _followRange = 0;
        ChangeState(Walk);
    }

    // Check if the mouse has reached the target
    private bool TargetReached()
    {
        return transform.position == Target.transform.position;
    }

    protected override void Update()
    {
        base.Update();

        // Boss  keep moving even when attacked by a cat
        if (!(Entity as Mouse).IsBoss)
        {
            if (_currentState is not State_Freeze && Entity.IsAttacked) ChangeState(Freeze);
        }

        if (TargetReached())
        {
            if (Target != GameManager.Instance.Cheese.gameObject)
            {
                Target = VisitedNextRoom?.Invoke(transform.position);
            }
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}