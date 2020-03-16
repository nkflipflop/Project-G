public class WeaponTurret : WeaponBase {
	public WeaponTurret() {
		ReloadTime = 0;
		FireRate = .2f;
		Damage = 5;

		IsAutomatic = true;
		HasRecoil = true;

		MaxAmmo = 1;
		CurrentAmmo = MaxAmmo;
	}
}
