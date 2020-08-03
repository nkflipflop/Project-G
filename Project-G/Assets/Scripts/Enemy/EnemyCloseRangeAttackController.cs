using UnityEngine;

public class EnemyCloseRangeAttackController : MonoBehaviour {

    [SerializeField] private int _damage = 3;
    private bool _canAttack = true;
    private CircleCollider2D _attackRangeCollider;

    private void Start() {
        _attackRangeCollider = gameObject.GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
		if (_canAttack && other.gameObject.CompareTag("Player")) {
            _canAttack = false;
            other.gameObject.GetComponent<HealthController>().TakeDamage(_damage);
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
