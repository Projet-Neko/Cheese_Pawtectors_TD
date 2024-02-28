using UnityEngine;

public abstract class State
{
    protected CatBrain _brain;

    public virtual void OnEnter(CatBrain brain)
    {
        _brain = brain;
    }

    public virtual void OnUpdate()
    {
        //
    }

    public virtual void OnExit()
    {
        //
    }

    protected bool IsInFollowRange()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(_brain.transform.position, _brain.FollowRange);

        foreach (Collider2D target in targets)
        {
            if (target.gameObject.layer == 8)
            {
                _brain.Target = target.gameObject;
                return true;
            }
        }

        return false;
    }

    protected bool IsInAttackRange()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(_brain.transform.position, _brain.AttackRange);

        foreach (Collider2D target in targets)
        {
            if (target.gameObject == _brain.Target) return true;
        }

        return false;
    }
}