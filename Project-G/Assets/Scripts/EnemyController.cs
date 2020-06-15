using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	public GameObject Target;
	private AStarPathfinding _aStar;
	[SerializeField] private float _speed = 1f;

	private Vector3 _targetPos;
	private float _distanceBtwTarget;
	private int _maxPathLength = 6;

	public DamageHelper DamageHelper;
	private Animator _animator;

	private Vector2 _sightDir;
	private bool _isAttacking = false;
	private float _attackRange = 0.4f;
	[SerializeField] private float _damage = 3f;

	private CircleCollider2D _circleCollider;

	/*  
	*   IMPORTANT NOTES:
	*   Enemy needs to be initiated in the dungeon
	*   Don't forget to set _aStar.StartPos to its value
	*   StartPos and GoalPos will no be set to anything in Start(). Delete them in the future.
	*/

	// Start is called before the first frame update
	void Start() {
		_animator = gameObject.GetComponent<Animator>();
		_circleCollider = gameObject.GetComponent<CircleCollider2D>();

		_aStar = gameObject.GetComponent<AStarPathfinding>();
		_aStar.StartPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
		_aStar.GoalPos = new Vector3Int(Mathf.RoundToInt(Target.transform.position.x), Mathf.RoundToInt(Target.transform.position.y), Mathf.RoundToInt(Target.transform.position.z));
		_targetPos = new Vector3Int(1000, 0, 0);        // null value

		InvokeRepeating("CheckTargetPosition", 5.0f, 0.5f);     // run this function every 0.5 sec
	}

	// Update is called once per frame
	void FixedUpdate() {
		if (!DamageHelper.IsDead) {
			EnemyAnimate();
			Movement();
		}
	}

	private void Movement() {
		_distanceBtwTarget = (Target.transform.position - transform.position).magnitude;
		_sightDir = Target.transform.position - transform.position;

		if (_distanceBtwTarget < _attackRange) {			// close enough to attack
			_targetPos = new Vector3Int(1000, 0, 0);
			_isAttacking = true;
		}
		else {
			_isAttacking = false;
			if (_distanceBtwTarget < 0.6f)				// get closer to attack
				_targetPos = Target.transform.position;
			else if (_aStar.Path != null && _aStar.Path.Count > 0 && _aStar.Path.Count <= _maxPathLength) {
				if (_targetPos == new Vector3Int(1000, 0, 0) || transform.position == _targetPos) {
					_targetPos = _aStar.Path.Pop();
				}
			}
			else if (_aStar.Path != null && _aStar.Path.Count > _maxPathLength)
				_targetPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);       // if not chasing the Target, stay where you are
		}

		if (_targetPos != new Vector3Int(1000, 0, 0)) {
			transform.position = Vector3.MoveTowards(transform.position, _targetPos, Time.deltaTime * _speed);      			// moving the enemy towards to target
		}
	}

	private void CheckTargetPosition() {
		Vector3Int TargetPos = new Vector3Int(Mathf.RoundToInt(Target.transform.position.x), Mathf.RoundToInt(Target.transform.position.y), Mathf.RoundToInt(Target.transform.position.z));
		if (_aStar.GoalPos != TargetPos) {
			_aStar.Current = null;
			_aStar.StartPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
			_aStar.GoalPos = TargetPos;
			_aStar.Path = null;
			_aStar.PathFinding();
		}
	}

	private void EnemyAnimate() {
		_animator.SetBool("IsAttacking", _isAttacking);
		_animator.SetFloat("Horizontal", _sightDir.x); 
		_animator.SetFloat("Vertical", _sightDir.y);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<DamageHelper>().TakeDamage(_damage);
        }
	}

	public void EnableCollider() {
		_circleCollider.enabled = true;
	}

	public void DisableCollider() {
		_circleCollider.enabled = false;
	}
}
