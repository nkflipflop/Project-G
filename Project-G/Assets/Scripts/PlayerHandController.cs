using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandController : MonoBehaviour {

	public SpriteRenderer RendererPlayer;

	private Vector3 _mousePosition;
	private Vector3 _weaponPosition = new Vector3 (0, 0, 0);
	private WeaponBase _currentWeapon;
	private WeaponBase _newWeapon;

	
	private bool _takeWeapon;
	private bool _canTake = false;


	private void InterractWithNewWeapon(){
		if(_canTake){
			_takeWeapon = Input.GetKeyDown("e");
			//Debug.Log("Çok istiyorsan 'E' ye bas. ;(");
			if(_takeWeapon) {
				// Dropping the current weapon
				_currentWeapon.transform.SetParent(null);
				// Equipping the new weapon
				EquipWeapon(_newWeapon);
			}
		}
	}

	// Equips the new weapon from ground
	private void EquipWeapon(WeaponBase weapon){
			weapon.transform.SetParent(transform, false);
			weapon.transform.localPosition = _weaponPosition;
			weapon.transform.rotation = transform.rotation;

			_currentWeapon = weapon;
	}

	// Adjusts the sorting order of the weapon according to mouse position (player's direction)
	private void AdjustSortingOrder(){
		Vector2 mouseDir = _mousePosition - transform.parent.position;
		float horizontal = mouseDir.x;
		float vertical = mouseDir.y;
		float slope = horizontal / vertical;
		
		int sortingOrder = RendererPlayer.sortingOrder;
		if (-1 < slope && slope < 1 && vertical > 0)	
			sortingOrder -= 2;
		else	
			sortingOrder += 2;

		_currentWeapon.SetSortingOrder(sortingOrder);
	}

	// Aims the weapon
	private void AimWeapon() {
		// Flipping hand position and weapon direction
		bool onRight = (_mousePosition.x - transform.position.x) > 0 ? true : false;
		_currentWeapon.Flip(onRight);
		if (onRight)
			transform.localPosition = new Vector3(-Mathf.Abs(transform.localPosition.x), transform.localPosition.y, 0f);
		else
			transform.localPosition = new Vector3(Mathf.Abs(transform.localPosition.x), transform.localPosition.y, 0f);

		// Getting mouse position and direction of player to mouse
		Vector3 aimDirection = (_mousePosition - _currentWeapon.transform.position).normalized;

		// Rotating the current weapon
		float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
		_currentWeapon.transform.eulerAngles = new Vector3(0, 0, angle);
	}

	private void ControlWeapon(){
		if(_currentWeapon != null){
			_currentWeapon.WeaponUpdate();
			AimWeapon();
			AdjustSortingOrder();
		}
	}

	// Gets Inputs
	private void GetInputs() {
		// Taking mouse position
		_mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		_mousePosition.z = 0f;
	}

	private void Start() {
		_currentWeapon = transform.GetChild(0).GetComponent<WeaponBase>();
		_currentWeapon.transform.localPosition = _weaponPosition;
	}
	private void Update() {
		GetInputs();
		// Controls the weapon
		ControlWeapon();
		InterractWithNewWeapon();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Weapon")){
			_canTake = true;
			_newWeapon = other.gameObject.GetComponent<WeaponBase>();
		}
	}
	
	private void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.CompareTag("Weapon")){
			_canTake = false;
			_newWeapon = null;    
		}
	}


}
