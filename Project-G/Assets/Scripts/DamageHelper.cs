using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHelper : MonoBehaviour {
    [SerializeField] private float _health = 25;

    private bool _isDead = false;

    public Dissolve DissolveEffect;

    public bool IsDead { get { return _isDead; } }
    
	public void TakeDamage(float damage) {
		_health -= damage;
		if (_health <= 0f) {
			DissolveEffect.IsDissolving = true;
            _isDead = true;
		}
	}
}
