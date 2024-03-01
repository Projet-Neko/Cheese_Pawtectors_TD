using UnityEngine;

public class Brain : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private Entity _entity;

    public GameObject Target { get; set; }
    public Collider2D Collider => _collider;
    public Entity Entity => _entity;

    public float AttackRange => _attackRange;
    public float FollowRange => _followRange;

    protected float _attackRange;
    protected float _followRange;

    private State _currentState;

    // All states
    public State Storage = new SStorage();
    public State Idle = new SIdle();
    public State Follow = new SFollow();
    public State Attack = new SAttack();
    public State Sleep = new SSleep();
    public State Walk = new SWalk();

    private void Update()
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, FollowRange);
    }
}