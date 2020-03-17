public class WeaponTurret : WeaponBase {
	public WeaponTurret() {
		ReloadTime = 0;
		FireRate = .2f;
		Damage = 5;

		HasRecoil = true;

		MaxAmmo = 1;
		CurrentAmmo = MaxAmmo;
	}

	public override void TriggerWeapon(){
		Fire();
	}
}
