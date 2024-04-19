using UnityEngine;

public class Brain : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] protected Collider2D _collider;
    [SerializeField] protected Entity _entity;

    public GameObject Target { get; set; }
    public BoxCollider Room { get; set; }
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
    public State Freeze = new State_Freeze();

    protected virtual void Update()
    {
        _currentState?.OnUpdate();
    }

    public void ChangeState(State newState)
    {
        _currentState?.OnExit();
        _currentState = newState;
        _currentState.OnEnter(this);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _followRange);
    }

    public void SpriteDirection(Vector3 destination)
    {
        // Calculer la direction du mouvement
        Vector3 moveDirection = (destination - transform.position).normalized;

        // Déterminer le secteur dans lequel se trouve le mouvement
        int sector = GetMovementSector(moveDirection);

        // Choisir le sprite en fonction du secteur
        Entity.Renderer.sprite = (Entity as Cat).Sprites[sector];
    }

    // Méthode pour déterminer le secteur du mouvement
    private int GetMovementSector(Vector3 moveDirection)
    {
        // Convertir le vecteur de direction en angle
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

        // Ajouter 360 degrés pour éviter les angles négatifs
        if (angle < 0)
        {
            angle += 360;
        }

        // Diviser le cercle en 8 secteurs et assigner un secteur pour chaque direction
        if (angle >= 22.5f && angle < 67.5f)
        {
            return 2; // Nord-Est
        }
        else if (angle >= 67.5f && angle < 112.5f)
        {
            return 1; // Nord
        }
        else if (angle >= 112.5f && angle < 157.5f)
        {
            return 3; // Nord-Ouest
        }
        else if (angle >= 157.5f && angle < 202.5f)
        {
            return 7; // Ouest
        }
        else if (angle >= 202.5f && angle < 247.5f)
        {
            return 6; // Sud-Ouest
        }
        else if (angle >= 247.5f && angle < 292.5f)
        {
            return 4; // Sud
        }
        else if (angle >= 292.5f && angle < 337.5f)
        {
            return 5; // Sud-Est
        }
        else
        {
            return 0; // Est
        }
    }
}