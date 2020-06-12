using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHelper : MonoBehaviour {
    [SerializeField] private float _health = 25;

    private bool _isDead = false;

    public Dissolve DissolveEffect;

    public bool IsDead { get { return _isDead; } }

    // Update is called once per frame
    private void Update() {
        if (_health <= 0f) {
            _isDead = true;
			DissolveEffect.IsDissolving = true;
		}
    }

    
	public void TakeDamage(float damage) {
		_health -= damage;
		if (_health <= 0f) {
			DissolveEffect.IsDissolving = true;
		}
	}
}
