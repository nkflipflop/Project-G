using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

	public float lifeTime = 50;
	public float speed;
	public int damage;
	
	private SpriteRenderer _renderer;

	
    public GameObject destroyEffect;
	public Sprite hittedSprite;

	private void Start() {
		_renderer = GetComponent<SpriteRenderer>();
		// Destroying when hit
		StartCoroutine(DestroyProjectile());
	}

	private void Update() {
		// Ray collider controlling
		RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector3.right, .1f);
		
		// Range controlling for arrow
		if (lifeTime > 0) {
			lifeTime -= 1;
			// When ray collides with another collider
			if ( hitInfo.collider != null && hitInfo.collider.IsTouchingLayers(LayerMask.GetMask("Environment", "Enemy"))) {
				// If it is enemy
				if (hitInfo.collider.CompareTag("Enemy"))
					hitInfo.collider.GetComponent<EnemyController>().TakeDamage(damage);
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
			Debug.Log(transform.rotation);
			Instantiate(destroyEffect, transform.position, transform.rotation, transform);
		}
	}
	
	// Waits for seconds.
	IEnumerator DestroyProjectile() {
		yield return new WaitForSeconds(5);
		Destroy(gameObject);
	}
}
