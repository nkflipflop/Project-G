using System.Collections;
using UnityEngine;

public class HealthController : MonoBehaviour {
    [SerializeField] private int _health = 25;

    private bool _isDead = false;

    public Dissolve DissolveEffect;
	public SpriteRenderer Renderer;
	public CapsuleCollider2D HitboxCollider;

    public bool IsDead { get { return _isDead; } }
	public int Health { 
		get { return _health; } 
		set { _health = value; } 
	}
    
	public void TakeDamage(int damage) {
		_health -= damage;

		if (_health <= 0) {
			_health = 0;
			DissolveEffect.IsDissolving = true;
            _isDead = true;
			HitboxCollider.enabled = false;
		}
				
		// Damage effect on sprite
		StartCoroutine(DamageEffect());
		// Sound Effect
		SoundManager.PlaySound(SoundManager.Sound.CharacterHit, transform.position);
	}

	IEnumerator DamageEffect() {
		Renderer.color = new Color32(255, 55, 0, 255);
		yield return new WaitForSeconds(0.2f);
		Renderer.color = Color.white;	
	}

	public void Heal(int healValue) {
		_health += healValue;

		if(_health > 100) 
			_health = 100;
		
		// Heal effect on sprite
		StartCoroutine(HealEffect());
	}

	IEnumerator HealEffect() {
		Renderer.color = new Color32(55, 255, 0, 255);
		yield return new WaitForSeconds(0.2f);
		Renderer.color = Color.white;	
	}
}
