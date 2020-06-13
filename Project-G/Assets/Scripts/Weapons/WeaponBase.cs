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
	
	public int CurrentAmmo { get; set; }
	public SpriteRenderer Renderer;
	public SpriteRenderer LeftHand;
	public SpriteRenderer RightHand;
	public GameObject Projectile;
	public GameObject FireEffect;
	
	// Changes layer of the sprite
	public void SetSortingOrder(int order){
		Renderer.sortingOrder = order;
		if(LeftHand)
			LeftHand.sortingOrder = order + 2;

		if(RightHand)
			RightHand.sortingOrder = order + 2;
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
		_timeBtwShots = FireRate;

		Transform shootPoint = transform.GetChild(0);
		// Recoiling the weapon
		if (HasRecoil) _weaponRecoiler.AddRecoil();
		// Creating Fire Effect
		StartCoroutine(FireEffector(shootPoint.position));
		// Creating projectile
		Instantiate(Projectile, shootPoint.position, shootPoint.rotation);
		
		Projectile.GetComponent<ProjectileController>().ShotByPlayer = (transform.root.tag == "Player");
	}

	public virtual void TriggerWeapon() {
		if (Input.GetMouseButtonDown(0) && CurrentAmmo > 0){
			Fire();
		}
	}

	// Update the weapon
	public void WeaponUpdate() {
		// For firing
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
