using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPistol : WeaponBase {
	public WeaponPistol() {
		ReloadTime = 3f;
		FireRate = .1f;
		Damage = 2;

		HasRecoil = true;

		MaxAmmo = 7;
		CurrentAmmo = MaxAmmo;
	}
}
