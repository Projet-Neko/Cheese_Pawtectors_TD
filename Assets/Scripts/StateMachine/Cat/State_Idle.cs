using UnityEngine;

public class State_Idle : State
{
    public override void OnEnter(Brain brain)
    {
        base.OnEnter(brain);
        SetRoom();
        (_brain as CatBrain).StartIdleMovement();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if ((_brain.Entity as Cat).IsInStorageMode) _brain.ChangeState(_brain.Sleep);

        if (!IsInFollowRange()) return;
        _brain.ChangeState(_brain.Follow);
        return;
    }

    public override void OnExit()
    {
        base.OnExit();

        (_brain as CatBrain).StopIdleMovement();
    }

    private void SetRoom()
    {
        while (_brain.Room == null)
        {
            if (_brain.Entity.transform.parent.gameObject.TryGetComponent(out BoxCollider collider))
            {
                _brain.Room = collider;
            }
        }
    }
}