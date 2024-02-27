using UnityEngine;

public class CatBrain : MonoBehaviour
{
    private State _currentState;
    protected GameObject _target;

    private State _idle = new SIdle();
    private State _follow = new SFollow();
    private State _attack = new SAttack();
    private State _sleep = new SSleep();

    private void Awake()
    {
        ChangeState(_idle);
    }

    private void Update()
    {
        _currentState?.OnUpdate();

        Collider[] targets = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, 4);
        //Debug.Log(targets.Length);

        if (_currentState != _idle && targets.Length == 0) return;
        _target = targets[0].gameObject;
        ChangeState(_follow);
        Debug.Log($"New state : {_currentState}.");
    }

    private void ChangeState(State newState)
    {
        _currentState?.OnExit();
        _currentState = newState;
        _currentState.OnEnter();
    }
}