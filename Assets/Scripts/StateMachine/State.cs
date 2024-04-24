using UnityEngine;

public abstract class State
{
    protected Brain _brain;

    public virtual void OnEnter(Brain brain)
    {
        _brain = brain;

        if (_brain.Entity is Cat) Mod_Waves.OnWaveReload += M_Wave_OnWaveReload;

        //if (_brain.Entity is Cat && !(_brain.Entity as Cat).IsInStorageMode) Debug.Log($"New state for {_brain.Entity.name} : {this}");
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
            if (target.gameObject.layer == 8)
            {
                if (!_brain.Room.bounds.Contains(target.transform.position)) continue;

                Mouse m = target.GetComponentInParent<Mouse>();

                if ((m.IsBoss || m.Attacker == null) && _brain.Target == null)
                {
                    m.Attacker = _brain.Entity as Cat;
                    _brain.Target = target.gameObject;
                    //Debug.Log($"<color=red>{_brain.Entity.name} is targeting {m.name}</color>");
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
            if (_brain.Entity is Cat && !_brain.Room.bounds.Contains(target.transform.position)) continue;
            if (target.gameObject == _brain.Target)
            {
                //Debug.Log($"{_brain.Entity.name} is attacking {target.gameObject.name}");
                return true;
            }
        }

        return false;
    }

    protected void FollowTarget()
    {
        _brain.transform.position = Vector3.MoveTowards(_brain.transform.position, _brain.Target.transform.position, _brain.Entity.Speed * Time.deltaTime);

        Vector3 newPosition = _brain.transform.localPosition;
        newPosition.y = 0.2f;
        _brain.transform.localPosition = newPosition;

        if (_brain.Entity is not Cat) return;
        _brain.SpriteDirection(_brain.Target.transform.position);
    }

    protected void M_Wave_OnWaveReload()
    {
        if (_brain.Entity is not Cat) return;
        (_brain.Entity as Cat).WakeUp();
        if (!(_brain.Entity as Cat).IsInStorageMode) _brain.ChangeState(_brain.Idle);
    }
}