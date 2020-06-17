using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

	public SpriteRenderer Renderer;
    public GameObject DestroyEffect;
	public Sprite HittedSprite;

	private float _lifetime = 50;
	private float _speed = 30;
	[SerializeField] private int _damage = 2;
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
		RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, direction, .25f, ShotByPlayer ? _hittableLayersByPlayer : _hittableLayersByEnemy);
		//Debug.DrawRay(transform.position, direction, Color.green);
		// Range controlling for arrow	
		if (_lifetime > 0) {
			_lifetime -= 1;
			// When ray collides with another collider
			if ( hitInfo.collider != null) {
				// If it is enemy
				if (hitInfo.collider.CompareTag("Player") || hitInfo.collider.CompareTag("Enemy")) {
					hitInfo.collider.gameObject.GetComponent<DamageHelper>().TakeDamage(_damage);
				}
				// Stopping the projectile
				_lifetime = 0;
			}
			else {
				transform.Translate(Vector3.right * _speed * Time.deltaTime);
			}
		}
		else if (_lifetime == 0){
			_lifetime -= 1;
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
