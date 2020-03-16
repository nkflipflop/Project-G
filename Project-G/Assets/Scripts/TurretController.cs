using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {
	
	private float _health = 25;
	private Transform _shootPoint;
	[SerializeField] private WeaponBase _weapon = null;
	public GameObject Target;

	// Start is called before the first frame update
	void Start() {
		_shootPoint = _weapon.transform.GetChild(0);
	}

	private void Shoot() {
		if (Target != null) {
			bool onRight = (Target.transform.position.x - transform.position.x) > 0 ? true : false;
			_weapon.GetComponent<WeaponBase>().flip(onRight);	// change this
			if (onRight)
				_weapon.transform.localPosition = new Vector3(-Mathf.Abs(_weapon.transform.localPosition.x), _weapon.transform.localPosition.y, 0f);
			else
				_weapon.transform.localPosition = new Vector3(Mathf.Abs(_weapon.transform.localPosition.x), _weapon.transform.localPosition.y, 0f);

			// Getting player position
			Vector3 aimDirection = (Target.transform.position - transform.position).normalized;
			// Rotating the current weapon
			float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
			_weapon.transform.eulerAngles = new Vector3(0, 0, angle);

			_weapon.Shoot();
		}
	}

	public void TakeDamage(float damage) {
		_health -= damage;
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
