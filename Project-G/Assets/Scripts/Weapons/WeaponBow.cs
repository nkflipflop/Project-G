using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBow : WeaponBase {
	public WeaponBow() {
		ReloadTime = 2;
		FireRate = .2f;
		Damage = 2;

		HasRecoil = true;

		MaxAmmo = 7;
		CurrentAmmo = MaxAmmo;
	}
}
