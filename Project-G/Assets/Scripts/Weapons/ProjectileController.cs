using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

	public SpriteRenderer Renderer;
	public GameObject DestroyEffect;
	public Sprite NoneSprite;
	public GameObject Light;
	
	[Header("Bullet Properties")]
	[SerializeField] private int _endurance = 1;
	[SerializeField] private float _speed = 10;
	[SerializeField] private int _damage = 2;
	private float _lifetime = 100;
	public bool ShotByPlayer = false;
	

	private int _enemyLayer = 8;
	private int _environmentLayer = 9;
	private int _playerLayer = 10;
	private int _destructibleObjectLayer = 11;
	private int _shieldLayer = 12;
	private LayerMask _hittableLayersByPlayer;
	private LayerMask _hittableLayersByEnemy;

	private void Start() {
		// Destroying when hit
		StartCoroutine(DestroyProjectile());
		
		_hittableLayersByPlayer = (1 << _enemyLayer) | (1 << _environmentLayer) | (1 << _destructibleObjectLayer);
		_hittableLayersByEnemy = (1 << _playerLayer) | (1 << _environmentLayer) | (1 << _shieldLayer);
	}

	private void FixedUpdate() {
		// Ray collider controlling
		Vector3 direction = transform.rotation * Vector3.right;
		RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, direction, .25f, ShotByPlayer ? _hittableLayersByPlayer : _hittableLayersByEnemy);
		//Debug.DrawRay(transform.position, direction, Color.green);
		// Range controlling for bullet	
		if (_lifetime > 0 && _endurance > 0) {
			_lifetime -= 1;
			// When ray collides with another collider
			if ( hitInfo.collider != null) {
				_endurance--;
				// If it is damageable
				if (hitInfo.collider.CompareTag("Player") || hitInfo.collider.CompareTag("Enemy") || hitInfo.collider.CompareTag("Breakable")) {
					hitInfo.collider.gameObject.GetComponent<HealthController>().TakeDamage(_damage);
				}

				if (_endurance < 1) {
					// Stopping the projectile
					_lifetime = 0;
				}
				else {
					_speed *= 0.6f;			// after hitting an enemy or an object, slow down the bullet
				}
			}
			else {
				transform.Translate(Vector3.right * _speed * Time.deltaTime);
			}
		}
		else if (_lifetime == 0) {
			_lifetime -= 1;
			Renderer.sprite = NoneSprite;
			Light.SetActive(false);
			
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
