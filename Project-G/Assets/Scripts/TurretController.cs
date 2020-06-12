using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {
	private int _environmentLayer = 9;
	private int _playerLayer = 10;
	private Transform _shootPoint;
	[SerializeField] private WeaponBase _weapon = null;
	public GameObject Target;
	private LayerMask _hittableLayersByEnemy;

	// Start is called before the first frame update
	void Start() {
		_shootPoint = _weapon.transform.GetChild(0);
		_hittableLayersByEnemy = (1 << _playerLayer) | (1 << _environmentLayer);
	}

	private void FixedUpdate() {
		CheckPlayerInRange();
	}

	private void CheckPlayerInRange() {
		if (Target != null) {
			Vector3 direction = Target.transform.position - transform.position;
			direction.y -= .23f;
			RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, direction, 4f, _hittableLayersByEnemy);
			Debug.DrawRay(transform.position, direction, Color.blue, .1f);
	
			if (hitInfo.collider != null) {
				if (hitInfo.collider.tag == "Player") {
					Shoot();
				}
			}
		}
	}

	private void Shoot() {
		if (Target != null) {
			// Getting player position
			Vector3 targetPos = new Vector3(Target.transform.position.x, Target.transform.position.y - 0.2f, Target.transform.position.z);
			Vector3 aimDirection = (targetPos - _weapon.transform.position).normalized;
			// Rotating the current weapon
			float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
			_weapon.transform.eulerAngles = new Vector3(0, 0, angle);

			_weapon.WeaponUpdate();
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
