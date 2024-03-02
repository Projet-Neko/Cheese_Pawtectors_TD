using UnityEngine;

public class Brain : MonoBehaviour
{
    [Header("Dependencies")]
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
    public State Idle = new State_Idle();
    public State Follow = new State_Follow();
    public State Attack = new State_Attack();
    public State Sleep = new State_Sleep();
    public State Walk = new State_Walk();

    protected virtual void Update()
    {
        _currentState?.OnUpdate();
    }

    public void ChangeState(State newState)
    {
        _currentState?.OnExit();
        _currentState = newState;
        _currentState.OnEnter(this);
        //Debug.Log($"New state : {_currentState}.");
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _followRange);
    }
}