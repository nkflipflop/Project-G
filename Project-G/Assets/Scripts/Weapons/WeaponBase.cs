using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour {


	private WeaponRecoiler _weaponRecoiler;
	private SpriteRenderer _renderer;
	private float _timeBtwShots;

	protected float ReloadTime;
	protected float FireRate;
	protected bool IsAutomatic;
	protected bool HasRecoil;
	protected float Damage;
	protected int MaxAmmo;
	public int CurrentAmmo { get; protected set; }

	public GameObject Projectile;

	// Flips the sprite
	public void flip(bool flip) {
		_renderer.flipY = !flip; 
	}

	// Shoots the weapon
	public void Shoot() {

		Transform shootPoint = transform.GetChild(0);
		if (_timeBtwShots <= 0) {
			if (Input.GetMouseButtonDown(0) || IsAutomatic == true) {
				// Creating projectile
				if (HasRecoil) _weaponRecoiler.AddRecoil();
				Instantiate(Projectile, shootPoint.position, transform.rotation);
				_timeBtwShots = FireRate;
			}
		}
		else {
			_timeBtwShots -= Time.deltaTime;
		}
	}


	private void Start() {
		_renderer = gameObject.GetComponent<SpriteRenderer>();
		_weaponRecoiler = GetComponent<WeaponRecoiler>();
	}
}
