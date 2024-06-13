using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum NPC_EnemyState { IDLE_STATIC, IDLE_ROAMER, IDLE_PATROL, INSPECT, ATTACK, FIND_WEAPON, KNOCKED_OUT, DEAD, NONE }

public class Enemy : MonoBehaviour
{
    public float inspectTimeout;
    public Projectile projectilePrefab;
    public Transform weaponPivot;
    public LayerMask hitTestLayer;
    public NPC_EnemyState idleState = NPC_EnemyState.IDLE_ROAMER;
    public Vector3 startingPos { get; set; }
    public Vector3 targetPos { get; set; }
    public float weaponRange= 20.0f;
    public float weaponActionTime= 0.025f;
    public float weaponTime= 0.05f;
    public float attackDelay = 1.0f; 
    public NavMeshAgent navMeshAgent { get; set; }
    private IEnemyState currentState;
    private Dictionary<NPC_EnemyState, IEnemyState> stateDictionary;
    private IAttackEnemy attackStrategy;
    private Timer weaponTimer = new Timer();
    public float projectileSpeed = 10.0f;  
    private ObjectPool<Projectile> projectilePool;
    private Coroutine attackCoroutine;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        startingPos = transform.position;
        projectilePool = new ObjectPool<Projectile>(projectilePrefab, 10);
       stateDictionary = new Dictionary<NPC_EnemyState, IEnemyState>
        {
            { NPC_EnemyState.IDLE_STATIC, new IdleStaticState() },
            { NPC_EnemyState.IDLE_ROAMER, new IdleRoamerState() },
            { NPC_EnemyState.INSPECT, new InspectState() },
            { NPC_EnemyState.ATTACK, new AttackState() }
        };
        
        ICommand attackCommand = new DefaultAttackCommand(weaponPivot, projectilePrefab.gameObject, gameObject, projectileSpeed, projectilePool);
        attackStrategy = new DefaultAttackEnemy(attackCommand);
        
        SetState(idleState);
    }
 
    void Update()
    {
        currentState?.UpdateState(this);
    }

    public void SetState(NPC_EnemyState newState)
    {
        currentState?.ExitState(this);
        currentState = stateDictionary[newState];
        currentState.EnterState(this);
    }

    public void StartAttacking()
    {
        if (attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(AttackRoutine());
        }
    }

    public void StopAttacking()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            attackStrategy.Attack(this);
            yield return new WaitForSeconds(attackDelay);

        }
    }

    public void RandomRotate()
    {
        float randomAngle = Random.Range(45, 180);
        float randomSign = Random.Range(0, 2) == 0 ? -1 : 1;
        transform.Rotate(0, randomAngle * randomSign, 0);
    }

    public bool HasReachedMyDestination()
    {
        return Vector3.Distance(transform.position, navMeshAgent.destination) <= 1.5f;
    }

    public void SetAlertPos(Vector3 newPos)
    {
        if (idleState != NPC_EnemyState.IDLE_STATIC)
        {
            SetTargetPos(newPos);
        }
    }

    public void SetTargetPos(Vector3 newPos)
    {
        targetPos = newPos;
        if (currentState.GetType() != typeof(AttackState))
        {
            SetState(NPC_EnemyState.INSPECT);
        }
    }
}
