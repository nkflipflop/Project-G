using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCloseRangeAttackController : MonoBehaviour {

    [SerializeField] private float _damage = 3f;
    private bool _canAttack = true;
    private CircleCollider2D _attackRangeCollider;

    private void Start() {
        _attackRangeCollider = gameObject.GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
		if (_canAttack && other.gameObject.CompareTag("Player")) {
            _canAttack = false;
            other.gameObject.GetComponent<DamageHelper>().TakeDamage(_damage);
        }
	}

    public void EnableCollider() {
		_attackRangeCollider.enabled = true;
	}

	public void DisableCollider() {
		_attackRangeCollider.enabled = false;
        _canAttack = true;
	}
}
