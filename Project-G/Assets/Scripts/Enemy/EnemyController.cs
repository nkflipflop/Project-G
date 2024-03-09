﻿using UnityEngine;
using Utilities;

public class EnemyController : MonoBehaviour
{
    public GameObject Target;
    private AStarPathfinding _aStar;
    [SerializeField] private float _speed = 1f;

    public float DistanceBtwTarget;
    private Vector3 _targetPos;
    private int _maxPathLength = 6;
    private Vector3Int _nullVector = new Vector3Int(0, 0, -1000); // null value

    public HealthController HealthController;
    private Animator _animator;
    private Vector2 _sightDir;
    private float _sightRange = 5f;
    private bool _isAttacking = false;
    private bool _isRunning = false;
    private float _attackRange = 0.45f;

    public EnemyCloseRangeAttackController AttackController;
    
    private readonly int Running = Animator.StringToHash("IsRunning");
    private readonly int IsAttacking = Animator.StringToHash("IsAttacking");
    private readonly int Horizontal = Animator.StringToHash("Horizontal");
    private readonly int Vertical = Animator.StringToHash("Vertical");

    public bool IsRunning => _isRunning;
    
    private void Start()
    {
        _animator = gameObject.GetComponent<Animator>();

        AStarSetup();
        Extensions.PeriodicAsync(async () => CheckTargetPosition(), 0.5f, 1.2f);        // run this function every 0.5 sec && wait 1.2 sec at the start
    }
    
    private void FixedUpdate()
    {
        if (!HealthController.IsDead)
        {
            EnemyAnimate();
            Movement();
        }
    }

    private void AStarSetup()
    {
        _aStar = gameObject.GetComponent<AStarPathfinding>();
        _aStar.StartPos =
            new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
        _aStar.GoalPos = new Vector3Int(Mathf.RoundToInt(Target.transform.position.x),
            Mathf.RoundToInt(Target.transform.position.y), Mathf.RoundToInt(Target.transform.position.z));
        _targetPos = _nullVector; // null value
    }

    private void Movement()
    {
        if (Target != null)
        {
            DistanceBtwTarget = Vector3.Distance(Target.transform.position, transform.position);
            _sightDir = Target.transform.position - transform.position;

            if (DistanceBtwTarget < _attackRange)
            {
                // close enough to attack
                _targetPos = _nullVector;
                _isAttacking = true;
            }
            else
            {
                _isAttacking = false;
                if (DistanceBtwTarget < 0.6f)
                {
                    // get closer to attack
                    _targetPos = Target.transform.position;
                }
                else if (_aStar.Path != null && _aStar.Path.Count > 0 && _aStar.Path.Count <= _maxPathLength)
                {
                    if (_targetPos == _nullVector || transform.position == _targetPos)
                    {
                        _targetPos = _aStar.Path.Pop();
                        _targetPos = new Vector2(_targetPos.x, _targetPos.y) + Random.insideUnitCircle * 0.15f;
                    }
                }
                else if (_aStar.Path != null && _aStar.Path.Count > _maxPathLength)
                    _targetPos =
                        Vector3Int.RoundToInt(transform.position); // if not chasing the Target, stay where you are
            }

            if (_targetPos != _nullVector)
            {
                transform.position =
                    Vector3.MoveTowards(transform.position, _targetPos,
                        Time.deltaTime * _speed); // moving the enemy towards to target
                _isRunning = (Vector3.Distance(transform.position, _targetPos) != 0);
            }
            else
            {
                _isRunning = false;
            }
        }
        else
        {
            // if the player is dead, stop attacking
            _isAttacking = false;
            _isRunning = false;
        }
    }

    private void CheckTargetPosition()
    {
        if (Target != null && gameObject.activeSelf)
        {
            Vector3Int TargetPos = Vector3Int.RoundToInt(Target.transform.position);
            bool targetInRange = DistanceBtwTarget < _sightRange;
            if ((_aStar.GoalPos != TargetPos && targetInRange) || targetInRange)
            {
                // if the player is in range, try to find a path				
                _aStar.SetupVariables(transform.position, TargetPos);
                _aStar.PathFinding();
            }
        }
    }

    private void EnemyAnimate()
    {
        _animator.SetBool(Running, _isRunning);
        _animator.SetBool(IsAttacking, _isAttacking);
        _animator.SetFloat(Horizontal, _sightDir.x);
        _animator.SetFloat(Vertical, _sightDir.y);
    }

    private void EnableAttackCollider()
    {
        AttackController.EnableCollider();
    }

    private void DisableAttackCollider()
    {
        AttackController.DisableCollider();
    }
}