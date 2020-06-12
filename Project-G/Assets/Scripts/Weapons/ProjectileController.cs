using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

	public float lifeTime = 50;
	public float speed;
	public int damage;
	public bool ShotByPlayer = true;
	
	private SpriteRenderer _renderer;
	
    public GameObject destroyEffect;
	public Sprite hittedSprite;

    private int _enemyLayer = 8;
    private int _environmentLayer = 9;
	private int _playerLayer = 10;
	private LayerMask _hittableLayersByPlayer;
	private LayerMask _hittableLayersByEnemy;

	private void Start() {
		_renderer = GetComponent<SpriteRenderer>();
		// Destroying when hit
		StartCoroutine(DestroyProjectile());

		_hittableLayersByPlayer = (1 << _enemyLayer) | (1 << _environmentLayer);
        _hittableLayersByEnemy = (1 << _playerLayer) | (1 << _environmentLayer);
	}

	private void Update() {
		// Ray collider controlling
		Vector3 direction = transform.rotation * Vector3.right;
		RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, direction, .1f, ShotByPlayer ? _hittableLayersByPlayer : _hittableLayersByEnemy);
		// Debug.DrawRay(transform.position, direction, Color.blue, .1f);


		// Range controlling for arrow	
		if (lifeTime > 0) {
			lifeTime -= 1;
			// When ray collides with another collider
			if ( hitInfo.collider != null) {
				// If it is enemy
				if (hitInfo.collider.CompareTag("Player") || hitInfo.collider.CompareTag("Enemy"))
					hitInfo.collider.GetComponent<DamageHelper>().TakeDamage(damage);
				// Stopping the projectile
				lifeTime = 0;
			}
			else {
				transform.Translate(Vector3.right * speed * Time.deltaTime);
			}
		}
		else if (lifeTime == 0){
			lifeTime -= 1;
			_renderer.sprite = hittedSprite;
			// Creating After Effect
			Instantiate(destroyEffect, transform.position, transform.rotation, transform);
		}
	}
	
	// Waits for seconds.
	IEnumerator DestroyProjectile() {
		yield return new WaitForSeconds(5);
		Destroy(gameObject);
	}
}
