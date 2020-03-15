using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPistol : WeaponBase {
	public WeaponPistol() {
		ReloadTime = 2;
		FireRate = .1f;
		Damage = 2;

		MaxAmmo = 7;
		CurrentAmmo = MaxAmmo;
	}
}
