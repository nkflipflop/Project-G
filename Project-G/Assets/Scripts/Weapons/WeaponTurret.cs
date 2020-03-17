public class WeaponTurret : WeaponBase {
	public WeaponTurret() {
		ReloadTime = .3f;
		FireRate = .3f;
		Damage = 5;

		HasRecoil = false;

		MaxAmmo = 1;
		CurrentAmmo = MaxAmmo;
	}

	public override void TriggerWeapon() {
		if (CurrentAmmo > 0) Fire();
	}
}
