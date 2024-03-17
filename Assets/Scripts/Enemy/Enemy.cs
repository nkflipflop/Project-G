using System.Threading;
using Pathfinding;
using Pooling;
using Pooling.Interfaces;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour, IPoolable, IPathfinder
{
    [field: SerializeField] public ObjectType Type { get; set; }
    
    [SerializeField] private Animator animator;
    [SerializeField] private float speed = 1f;

    public float distanceBtwTarget;
    private Vector3? targetPos;

    public HealthController HealthController;
    private Vector2 sightDir;
    private bool isAttacking;
    private bool isRunning;
    
    private const int MAX_PATH_LENGTH = 6;
    private const float SIGHT_RANGE = 5f;
    private const float ATTACK_RANGE = 0.45f;

    public EnemyCloseRangeAttackController attackController;
    
    private readonly int runningHash = Animator.StringToHash("IsRunning");
    private readonly int isAttackingHash = Animator.StringToHash("IsAttacking");
    private readonly int horizontalHash = Animator.StringToHash("Horizontal");
    private readonly int verticalHash = Animator.StringToHash("Vertical");

    public bool IsRunning => isRunning;

    private void OnDestroy()
    {
        TriggerCancellationToken();
    }

    private void FixedUpdate()
    {
        if (!HealthController.IsDead)
        {
            EnemyAnimate();
            Movement();
        }
    }

    private void Movement()
    {
        if (Target != null)
        {
            distanceBtwTarget = Vector3.Distance(Target.position, transform.position);
            sightDir = Target.position - transform.position;

            if (distanceBtwTarget < ATTACK_RANGE)
            {
                // close enough to attack
                targetPos = null;
                isAttacking = true;
            }
            else
            {
                isAttacking = false;
                if (distanceBtwTarget < 0.6f)
                {
                    // get closer to attack
                    targetPos = Target.position;
                }
                else if (AStar.Path?.Count > 0 && AStar.Path.Count <= MAX_PATH_LENGTH)
                {
                    if (!targetPos.HasValue || transform.position == targetPos.Value)
                    {
                        targetPos = AStar.Path.Pop();
                        targetPos = new Vector2(targetPos.Value.x, targetPos.Value.y) + Random.insideUnitCircle * 0.15f;
                    }
                }
                else if (AStar.Path != null && AStar.Path.Count > MAX_PATH_LENGTH)
                    targetPos = Vector3Int.RoundToInt(transform.position); // if not chasing the Target, stay where you are
            }

            if (targetPos.HasValue)
            {
                transform.position =
                    Vector3.MoveTowards(transform.position, targetPos.Value, Time.deltaTime * speed); // moving the enemy towards to target
                isRunning = (Vector3.Distance(transform.position, targetPos.Value) != 0);
            }
            else
            {
                isRunning = false;
            }
        }
        else
        {
            // if the player is dead, stop attacking
            isAttacking = false;
            isRunning = false;
        }
    }

    private void CheckTargetPosition()
    {
        if (Target != null && gameObject.activeSelf)
        {
            if (distanceBtwTarget < SIGHT_RANGE)
            {
                // if the player is in range, try to find a path				
                AStar.UpdateObjective(transform.position, Vector3Int.RoundToInt(Target.position));
                AStar.FindPath();
            }
        }
    }

    private void EnemyAnimate()
    {
        animator.SetBool(runningHash, isRunning);
        animator.SetBool(isAttackingHash, isAttacking);
        animator.SetFloat(horizontalHash, sightDir.x);
        animator.SetFloat(verticalHash, sightDir.y);
    }

    private void EnableAttackCollider()
    {
        attackController.EnableCollider();
    }

    private void DisableAttackCollider()
    {
        attackController.DisableCollider();
    }
    
    public void OnSpawn() { }

    public void OnReset()
    {
        TriggerCancellationToken();
    }
    
    #region A Star Setup
    
    private CancellationTokenSource cancellationTokenSource;
    private CancellationTokenSource CancellationTokenSource
    {
        get
        {
            cancellationTokenSource ??= new CancellationTokenSource();
            if (cancellationTokenSource.IsCancellationRequested)
            {
                cancellationTokenSource.Dispose();
                cancellationTokenSource = new CancellationTokenSource();
            }

            return cancellationTokenSource;
        }
    }
    
    public AStar AStar { get; set; }
    public Transform Target { get; set; }
    
    public void SetupPathfinding(Transform target, int[,] grid)
    {
        Target = target;
        AStarSetup(grid);
        Extensions.PeriodicAsync(async () => CheckTargetPosition(), 0.5f, 1.2f,
            CancellationTokenSource.Token);     // run this function every 0.5 sec && wait 1.2 sec at the start
    }
    
    private void AStarSetup(int[,] grid)
    {
        AStar ??= new AStar();
        AStar.Initialize(grid, Vector3Int.RoundToInt(transform.position), Vector3Int.RoundToInt(Target.position));
        targetPos = null;
    }

    private void TriggerCancellationToken()
    {
        if (CancellationTokenSource?.IsCancellationRequested == false)
        {
            CancellationTokenSource.Cancel();
        }
    }
    
    #endregion
}