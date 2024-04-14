using UnityEngine;

public abstract class State
{
    protected Brain _brain;

    public virtual void OnEnter(Brain brain)
    {
        _brain = brain;
        if (_brain.Entity is Cat) Mod_Waves.OnWaveReload += M_Wave_OnWaveReload;
    }

    public virtual void OnUpdate() { }
    public virtual void OnExit()
    {
        if (_brain.Entity is Cat) Mod_Waves.OnWaveReload -= M_Wave_OnWaveReload;
    }

    protected bool IsInFollowRange()
    {
        if (_brain.Entity is not Cat) return false;

        Collider2D[] targets = Physics2D.OverlapCircleAll(_brain.transform.position, _brain.FollowRange);

        foreach (Collider2D target in targets)
        {
            // layer "Enemy" (Mouse)
            if (target.gameObject.layer == 8)
            {
                Mouse m = target.GetComponentInParent<Mouse>();

                if ((m.IsBoss || m.Attacker == null) && _brain.Target == null)
                {
                    m.Attacker = _brain.Entity as Cat;
                    _brain.Target = target.gameObject;
                    //Debug.Log($"{_brain.Entity.name} is targeting {m.name}");
                    return true;
                }
                else if (m.Attacker == _brain.Entity as Cat) return true;
            }
        }

        _brain.Target = null;
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

    protected void FollowTarget()
    {
        _brain.transform.position = Vector3.MoveTowards(_brain.transform.position, _brain.Target.transform.position, _brain.Entity.Speed * Time.deltaTime);
    }

    protected void M_Wave_OnWaveReload()
    {
        if (_brain.Entity is not Cat) return;
        (_brain.Entity as Cat).WakeUp();
        _brain.ChangeState(_brain.Idle);
    }
}