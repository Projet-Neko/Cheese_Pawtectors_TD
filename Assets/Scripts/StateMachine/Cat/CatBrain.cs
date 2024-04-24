using System.Collections;
using UnityEngine;

public class CatBrain : Brain
{
    private Coroutine _idleMovement;

    private void Awake()
    {
        ChangeState(Sleep);
        _attackRange = (_entity.Renderer.bounds.size.x + _entity.Renderer.bounds.size.y) * 5;
        _followRange = (_entity.Renderer.bounds.size.x + _entity.Renderer.bounds.size.y) * 20;
    }

    protected override void Update()
    {
        base.Update();
        if (_currentState is not State_Sleep && !_entity.IsAlive()) ChangeState(Sleep);
        else if (_currentState is State_Sleep && !(_entity as Cat).IsInStorageMode && _entity.IsAlive()) ChangeState(Idle);
    }

    private void OnDestroy()
    {
        if (_idleMovement != null) StopCoroutine(_idleMovement);
    }

    public void StartIdleMovement()
    {
        _idleMovement = StartCoroutine(IdleMovement());
    }

    public void StopIdleMovement()
    {
        if (_idleMovement != null) StopCoroutine(_idleMovement);
    }

    private IEnumerator IdleMovement()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            Vector3 size = Room.bounds.size;

            Vector3 randomPointInBoxLocal = new Vector3(Random.Range(-size.x / 2f, size.x / 2f), 0.2f, Random.Range(-size.z / 2f, size.z / 2f));
            Vector3 randomPointInBoxWorld = transform.parent.TransformPoint(randomPointInBoxLocal);

            while (Vector3.Distance(transform.position, randomPointInBoxWorld) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, randomPointInBoxWorld, Entity.Speed * Time.deltaTime);
                SpriteDirection(randomPointInBoxWorld);
                yield return null;
            }

            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }

    public void SetRoom()
    {
        do
        {
            if (Entity.transform.parent.gameObject.TryGetComponent(out BoxCollider collider))
            {
                Room = collider;
            }
        }
        while (Room == null);
    }
}