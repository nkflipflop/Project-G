using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAssaultRifle : WeaponBase {
	public WeaponAssaultRifle() {
		ReloadTime = 5f;
		FireRate = .2f;
		Damage = 3;

		HasRecoil = true;

		MaxAmmo = 30;
		CurrentAmmo = MaxAmmo;
	}
	
	public override void TriggerWeapon(){
		if (Input.GetMouseButton(0) && CurrentAmmo > 0) {
			Fire();
		}
	}
}
