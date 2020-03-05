using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour {
	
	private SpriteRenderer _renderer;
	private float _timeBtwShots;

	protected float reloadTime;
	protected float fireRate;
	protected float damage;

	public GameObject projectile;

	public void flip(bool flip) {
		_renderer = gameObject.GetComponent<SpriteRenderer>();
		_renderer.flipY = !flip; 
	}

	// Shoots the weapon
	public void Shoot(){
		Transform shootPoint = transform.Find("Shoot Point");
		if (_timeBtwShots <= 0){
			if (Input.GetMouseButtonDown(0)){
				// Creatimg projectile
				Instantiate(projectile, shootPoint.position, transform.rotation);
				_timeBtwShots = fireRate;
			}
		}
		else {
			_timeBtwShots -= Time.deltaTime;
		}
	}
}
