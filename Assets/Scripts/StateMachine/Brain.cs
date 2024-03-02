using UnityEngine;

public class Brain : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] private Entity _entity;

    public GameObject Target { get; set; }
    public Entity Entity => _entity;

    public float AttackRange => _attackRange;
    public float FollowRange => _followRange;

    protected float _attackRange;
    protected float _followRange;
    protected State _currentState;

    // All states
    public State Storage = new SStorage();
    public State Idle = new SIdle();
    public State Follow = new SFollow();
    public State Attack = new SAttack();
    public State Sleep = new SSleep();
    public State Walk = new SWalk();

    protected virtual void Update()
    {
        _currentState?.OnUpdate();
    }

    public void ChangeState(State newState)
    {
        _currentState?.OnExit();
        _currentState = newState;
        _currentState.OnEnter(this);
        Debug.Log($"New state : {_currentState}.");
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _followRange);
    }
}