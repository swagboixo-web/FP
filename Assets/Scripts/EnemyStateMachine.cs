using UnityEngine;

// Clean, modular arcade state architecture
public interface IEnemyState
{
    void OnEnter(EnemyStateMachine enemy);
    void OnUpdate(EnemyStateMachine enemy);
    void OnExit(EnemyStateMachine enemy);
}

public class EnemyStateMachine : MonoBehaviour
{
    [Header("Enemy Attributes")]
    public float moveSpeed = 3f;
    public float attackRange = 10f;
    public int scoreValue = 10;

    [HideInInspector] public Transform player;
    [HideInInspector] public Vector3 spawnPosition;
    private Rigidbody rb;

    // Define the specific states for a Sky Force arcade shooter
    public enum EnemyState { Spawning, MovingPattern, ChasingPlayer, Attacking, Fleeing }
    [Header("Current State Configuration")]
    public EnemyState initialState = EnemyState.MovingPattern;

    private IEnemyState currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spawnPosition = transform.position;

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        // Initialize into our starting arcade state
        SetState(initialState);
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.OnUpdate(this);
        }
    }

    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null) currentState.OnExit(this);
        currentState = newState;
        if (currentState != null) currentState.OnEnter(this);
    }

    // Helper method to set state easily via the enum
    public void SetState(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.MovingPattern:
                ChangeState(new ArcadePatternState());
                break;
            case EnemyState.ChasingPlayer:
                ChangeState(new ArcadeChaseState());
                break;
            default:
                ChangeState(new ArcadePatternState());
                break;
        }
    }

    public void MoveTowards(Vector3 target)
    {
        if (rb == null) return;
        Vector3 direction = (target - rb.position).normalized;
        direction.y = 0f; // Constrain to 2.5D shooting plane

        Vector3 targetPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);

        if (direction != Vector3.zero)
        {
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f));
        }
    }
}

// ===== EXPLICIT ARCADE STATES =====

public class ArcadePatternState : IEnemyState
{
    public void OnEnter(EnemyStateMachine enemy) { }
    public void OnUpdate(EnemyStateMachine enemy)
    {
        // Default Sky Force behavior: Fly downward along the Z-axis
        enemy.transform.Translate(Vector3.back * enemy.moveSpeed * Time.deltaTime, Space.World);

        // Transition Condition: If player gets close, switch to active tracking/chase
        if (enemy.player != null && Vector3.Distance(enemy.transform.position, enemy.player.position) < enemy.attackRange)
        {
            enemy.SetState(EnemyStateMachine.EnemyState.ChasingPlayer);
        }
    }
    public void OnExit(EnemyStateMachine enemy) { }
}

public class ArcadeChaseState : IEnemyState
{
    public void OnEnter(EnemyStateMachine enemy) { Debug.Log("Arcade Enemy Aggro Locked!"); }
    public void OnUpdate(EnemyStateMachine enemy)
    {
        if (enemy.player == null) return;

        // Actively track and fly towards the player coordinates
        enemy.MoveTowards(enemy.player.position);
    }
    public void OnExit(EnemyStateMachine enemy) { }
}