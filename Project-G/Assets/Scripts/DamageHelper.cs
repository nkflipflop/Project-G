using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamageHelper : MonoBehaviour {
    [SerializeField] private float _health = 25;

    private bool _isDead = false;

    public Dissolve DissolveEffect;
	public SpriteRenderer Renderer;

    public bool IsDead { get { return _isDead; } }
    
	public void TakeDamage(float damage) {
		_health -= damage;

		if (_health <= 0f) {
			DissolveEffect.IsDissolving = true;
            _isDead = true;
		}
				
		// Damage effect on sprite
		StartCoroutine(DamageEffect());
	}

	IEnumerator DamageEffect() {
		Renderer.color = new Color32(224, 74, 89, 255);
		yield return new WaitForSeconds(0.2f);
		Renderer.color = Color.white;	
	}	
}
