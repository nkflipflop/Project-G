public class WeaponTurret : WeaponBase {
	public WeaponTurret() {
		ReloadTime = 0;
		FireRate = .2f;
		Damage = 5;

		IsAutomatic = true;
		HasRecoil = false;

		MaxAmmo = 1;
		CurrentAmmo = MaxAmmo;
	}
}
