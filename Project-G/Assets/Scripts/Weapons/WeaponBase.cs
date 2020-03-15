using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour {
	
	private SpriteRenderer _renderer;
	private float _timeBtwShots;

	protected float ReloadTime;
	protected float FireRate;
	protected float Damage;
	protected int MaxAmmo;
	public int CurrentAmmo { get; protected set; }

	public GameObject Projectile;

	public void flip(bool flip) {
		_renderer = gameObject.GetComponent<SpriteRenderer>();
		_renderer.flipY = !flip; 
	}

	// Shoots the weapon
	public void Shoot(){
		Transform shootPoint = transform.Find("Shoot Point");
		if (_timeBtwShots <= 0){
			if (Input.GetMouseButtonDown(0)){
				// Creating projectile
				Instantiate(Projectile, shootPoint.position, transform.rotation);
				_timeBtwShots = FireRate;
			}
		}
		else {
			_timeBtwShots -= Time.deltaTime;
		}
	}
}
