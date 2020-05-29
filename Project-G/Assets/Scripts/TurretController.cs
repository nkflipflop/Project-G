using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {
	
	[SerializeField] private float _health = 25;
	private Transform _shootPoint;
	[SerializeField] private WeaponBase _weapon = null;
	public GameObject Target;

	public Dissolve DissolveEffect;

	// Start is called before the first frame update
	void Start() {
		_shootPoint = _weapon.transform.GetChild(0);
	}

	private void Update() {		// !!!! DELETE this func. It is only for testing the dissolve effect
		if (_health <= 0f) {
			DissolveEffect.IsDissolving = true;
		}
	}

	private void Shoot() {
		if (Target != null) {
			// Getting player position
			Vector3 aimDirection = (Target.transform.position - _weapon.transform.position).normalized;
			// Rotating the current weapon
			float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
			_weapon.transform.eulerAngles = new Vector3(0, 0, angle);

			_weapon.WeaponUpdate();
		}
	}

	public void TakeDamage(float damage) {
		_health -= damage;
		if (_health <= 0f) {
			DissolveEffect.IsDissolving = true;
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
			Shoot();
		}
	}

	private void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
			Shoot();
		}
	}
}
