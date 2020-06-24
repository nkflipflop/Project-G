using UnityEngine;

public class EnemyCloseRangeAttackController : MonoBehaviour {

    [SerializeField] private float _damage = 3f;

    private CircleCollider2D _attackRangeCollider;

    private void Start() {
        _attackRangeCollider = gameObject.GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<DamageHelper>().TakeDamage(_damage);
        }
	}

    public void EnableCollider() {
		_attackRangeCollider.enabled = true;
	}

	public void DisableCollider() {
		_attackRangeCollider.enabled = false;
	}
}
