using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

	public SpriteRenderer Renderer;
    public GameObject DestroyEffect;
	public Sprite HittedSprite;

	public float LifeTime = 50;
	public float Speed = 30;
	public int Damage;
	public bool ShotByPlayer = true;
	

    private int _enemyLayer = 8;
    private int _environmentLayer = 9;
	private int _playerLayer = 10;
	private LayerMask _hittableLayersByPlayer;
	private LayerMask _hittableLayersByEnemy;

	private void Start() {
		// Destroying when hit
		StartCoroutine(DestroyProjectile());

		_hittableLayersByPlayer = (1 << _enemyLayer) | (1 << _environmentLayer);
        _hittableLayersByEnemy = (1 << _playerLayer) | (1 << _environmentLayer);
	}

	private void Update() {
		// Ray collider controlling
		Vector3 direction = transform.rotation * Vector3.right;
		RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, direction, .1f, ShotByPlayer ? _hittableLayersByPlayer : _hittableLayersByEnemy);

		// Range controlling for arrow	
		if (LifeTime > 0) {
			LifeTime -= 1;
			// When ray collides with another collider
			if ( hitInfo.collider != null) {
				// If it is enemy
				if (hitInfo.collider.CompareTag("Player") || hitInfo.collider.CompareTag("Enemy"))
					hitInfo.collider.GetComponent<DamageHelper>().TakeDamage(Damage);
				// Stopping the projectile
				LifeTime = 0;
			}
			else {
				transform.Translate(Vector3.right * Speed * Time.deltaTime);
			}
		}
		else if (LifeTime == 0){
			LifeTime -= 1;
			Renderer.sprite = HittedSprite;
			
			// Creating After Effect
			Instantiate(DestroyEffect, transform.position, transform.rotation, transform);
		}
	}
	
	// Waits for seconds.
	IEnumerator DestroyProjectile() {
		yield return new WaitForSeconds(5);
		Destroy(gameObject);
	}
}
