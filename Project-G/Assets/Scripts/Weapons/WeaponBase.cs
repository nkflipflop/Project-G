using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour {


	private WeaponRecoiler _weaponRecoiler;
	private float _timeBtwShots;

	protected float ReloadTime;
	protected float FireRate;
	protected bool HasRecoil;
	protected float Damage;
	protected int MaxAmmo;
	
	public int CurrentAmmo { get; protected set; }
	public SpriteRenderer Renderer;
	public GameObject Projectile;
	public GameObject FireEffect;
	
	// Changes layer of the sprite
	public void SetSortingOrder(int order){
		Renderer.sortingOrder = order;
	}

	// Flips the sprite
	public void Flip(bool flip) {
		Renderer.flipY = !flip; 
	}

	// Fires the Weapon
	public void Fire() {
		Transform shootPoint = transform.GetChild(0);
		// Recoiling the weapon
		if (HasRecoil) _weaponRecoiler.AddRecoil();
		// Creating Fire Effect
		StartCoroutine(FireEffector(shootPoint.position));
		// Creating projectile
		Instantiate(Projectile, shootPoint.position, transform.rotation);

		CurrentAmmo -= 1;
		_timeBtwShots = FireRate;
	}

	public virtual void TriggerWeapon() {
		if (Input.GetMouseButtonDown(0) && CurrentAmmo > 0){
			Fire();
		}
	}

	// Update the weapon
	public void WeaponUpdate() {
		if (_timeBtwShots <= 0 && CurrentAmmo > 0)
			TriggerWeapon();
		else
			_timeBtwShots -= Time.deltaTime;

		// Reloading
		if (CurrentAmmo == 0) {
			StartCoroutine(ReloadWeapon());
			return;
		}
	}


	private void Start() {
		_weaponRecoiler = GetComponent<WeaponRecoiler>();
	}

	// Reloads the weapon
	IEnumerator ReloadWeapon() {
		yield return new WaitForSeconds(ReloadTime);
		CurrentAmmo = MaxAmmo;
	}

	// Creates, and then destroys Fire effect on the weapon
	IEnumerator FireEffector(Vector3 position) {
		GameObject fireEffect;
		fireEffect = Instantiate(FireEffect, position, transform.rotation);
		yield return new WaitForSeconds(1);
		Destroy(fireEffect);
	}
}
