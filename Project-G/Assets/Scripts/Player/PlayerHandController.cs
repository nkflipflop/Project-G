using UnityEngine;

public class PlayerHandController : HandControllerBase {
	public PlayerController PlayerController;

	private WeaponBase _newWeapon;
	//private bool _hasKey = false;			// dungeon_level exit key
	///private bool _onExitDoor = false;		// player is on the exit door or not
	private bool _takeWeapon;
	private bool _canTake = false;
	

	public override void SpecialStart() {
		AimDeviation = 0;

		GameObject weapon = Instantiate(GameConfigData.Instance.Weapons[DataManager.Instance.WeaponID]) as GameObject;		// instantiating player's weapon
		EquipWeapon(weapon.GetComponent<WeaponBase>());
	}

	public override void SpecialUpdate() {
		InterractWithNewWeapon();
		//InterractWithExitDoor();
		CharacterIsRunning = PlayerController.IsRun;
	}
	
	protected override Vector3 GetTargetPosition(){
		return Camera.main.ScreenToWorldPoint(TargetObject.transform.position);
	}
	
	private void InterractWithNewWeapon() {
		if(_canTake) {
			_takeWeapon = Input.GetKeyDown(KeyCode.E);
			//Debug.Log("Çok istiyorsan 'E' ye bas. ;(");
			if(_takeWeapon) {
				// Dropping the current weapon
				CurrentWeapon.transform.SetParent(null);
				// Equipping the new weapon
				EquipWeapon(_newWeapon);
			}
		}
	}

	// private void InterractWithExitDoor() {
	// 	if(_hasKey && _onExitDoor) {		// if the player has the key and on the door, he can open it by pressing 'E'
	// 		bool isDoorOpened = Input.GetKeyDown("e");
	// 		if (isDoorOpened) {
	// 			Debug.Log("Door opened. Next level.");
	// 		}
	// 	}
	// }

	// Equips the new weapon from ground
	public void EquipWeapon(WeaponBase weapon){
			weapon.transform.SetParent(transform, false);
			weapon.transform.localPosition = WeaponPosition;
			weapon.transform.rotation = transform.rotation;

			CurrentWeapon = weapon;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Weapon")){
			_canTake = true;
			_newWeapon = other.gameObject.GetComponent<WeaponBase>();
		}
		// else if (other.gameObject.CompareTag("Key")) {
		// 	other.gameObject.SetActive(false);
		// 	_hasKey = true;
		// }
	}
	
	private void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.CompareTag("Weapon")){
			_canTake = false;
			_newWeapon = null;    
		}
	}
}
