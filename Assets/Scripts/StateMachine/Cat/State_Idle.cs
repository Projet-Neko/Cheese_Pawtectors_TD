using UnityEngine;

public class State_Idle : State
{
    public override void OnUpdate()
    {
        base.OnUpdate();

        if ((_brain.Entity as Cat).IsInStorageMode) return;

        while (_brain.Room == null)
        {
            if (_brain.Entity.transform.parent.gameObject.TryGetComponent(out BoxCollider collider))
            {
                _brain.Room = collider;
            }
        }

        // TODO -> Add idle movements

        if (!IsInFollowRange()) return;
        _brain.ChangeState(_brain.Follow);
        return;
    }
}