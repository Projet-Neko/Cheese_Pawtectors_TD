using UnityEngine;
using UnityEngine.AI;

public class CatBrain : MonoBehaviour
{
    //[SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private Cat _cat;

    public GameObject Target {  get; set; }
    //public NavMeshAgent Agent => _agent;
    public Collider2D Collider => _collider;
    public Cat Cat => _cat;
    public float AttackRange => transform.localScale.x / 1.5f;
    public float FollowRange => transform.localScale.x * 4;

    private State _currentState;

    // All states
    public State Idle = new SIdle();
    public State Follow = new SFollow();
    public State Attack = new SAttack();
    public State Sleep = new SSleep();

    private void Awake()
    {
        ChangeState(Idle);
    }

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