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
            Vector3 center = Room.bounds.center;
            Vector3 size = Room.bounds.size;

            //Debug.Log(center);
            //Debug.Log(size);

            Vector3 randomPointInBox = new (
            Random.Range(center.x - size.x / 2f, center.x + size.x / 2f),
            Random.Range(center.y - size.y / 2f, center.y + size.y / 2f),
            Random.Range(center.z - size.z / 2f, center.z + size.z / 2f)
        );

            //Debug.Log(randomPointInBox);

            while (Vector3.Distance(transform.position, randomPointInBox) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, randomPointInBox, Entity.Speed * Time.deltaTime);
                SpriteDirection(randomPointInBox);
                yield return null;
            }

            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }
}