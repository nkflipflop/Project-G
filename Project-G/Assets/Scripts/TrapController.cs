using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour {

    private int _damage = 2;
    private float _coolDown = 0.8f;
    private bool _didDamage = false;
    private BoxCollider2D _boxCollider2D;
    // Start is called before the first frame update
    void Start() {
        _boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!_didDamage && other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<DamageHelper>().TakeDamage(_damage);
            _didDamage = true;
            StartCoroutine(ResetDidDamage());
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (!_didDamage && other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<DamageHelper>().TakeDamage(_damage);
            _didDamage = true;
            StartCoroutine(ResetDidDamage());
        }
    }

    public void EnableCollider() {
        _boxCollider2D.enabled = true;
    }

    public void DisableCollider() {
        _boxCollider2D.enabled = false;
    }

    IEnumerator ResetDidDamage() {
		yield return new WaitForSeconds(_coolDown);
		_didDamage = false;
	}
}
