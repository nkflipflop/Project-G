using System;
using System.Collections;
using UnityEngine;

public class WeaponBase : MonoBehaviour {

	public WeaponPrefab Weapon;
	public WeaponRecoiler WeaponRecoiler;
	public GameObject FireEffect;
	public SpriteRenderer WeaponRenderer;
	public SpriteRenderer LeftHandRenderer;
	public SpriteRenderer RightHandRenderer;
	public Transform ShotPoint;
	[NonSerialized] public int CurrentAmmo;

	private float _timeBtwShots;
	private bool _canTrigger = true;
	private bool _isReloading = false;
	
	
	private void Start() {
		CurrentAmmo = Weapon.MaxAmmo;
	}

	// Changes layer of the sprite
	public void SetSortingOrder(int order){
		WeaponRenderer.sortingOrder = order;
		if(LeftHandRenderer){
			LeftHandRenderer.sortingOrder = order + 2;
			RightHandRenderer.sortingOrder = order + 2;
		}
	}

	// Flips the sprite
	public void ScaleInverse() {
		Vector3 scale = transform.localScale;
		scale.y *= -1;
		transform.localScale = scale; 
	}

	// Fires the Weapon	
	public void Fire() {
		CurrentAmmo -= 1;
		_timeBtwShots = Weapon.FireRate;

		// Fire Effect
		Instantiate(FireEffect, ShotPoint.position, ShotPoint.rotation);
		// Recoiling the weapon
		if (Weapon.HasRecoil) WeaponRecoiler.AddRecoil();
		// Creating projectile
		GameObject tempProjectile = Instantiate(Weapon.Projectile, ShotPoint.position, ShotPoint.rotation);
		
		tempProjectile.GetComponent<ProjectileController>().ShotByPlayer = (transform.root.tag == "Player");
	}

	public void Trigger() {
		if (_canTrigger && _timeBtwShots <= 0 && CurrentAmmo > 0){
			Fire();
			_canTrigger = Weapon.Automatic == true ? true : false;			// if weapon is not automatic, you need to release trigger
		}
	}

	public void ReleaseTrigger(){
		_canTrigger = true;
	}

	// Update the weapon
	public void WeaponUpdate() {
		// For firing
		_timeBtwShots -= Time.deltaTime;

		// Reloading
		if (CurrentAmmo == 0 && !_isReloading) {
			StartCoroutine(ReloadWeapon());
			return;
		}
	}

	// Reloads the weapon
	IEnumerator ReloadWeapon() {
		_isReloading = true;
		yield return new WaitForSeconds(Weapon.ReloadTime);
		CurrentAmmo = Weapon.MaxAmmo;
		_isReloading = false;
	}
}
