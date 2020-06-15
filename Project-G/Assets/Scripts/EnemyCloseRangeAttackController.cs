using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCloseRangeAttackController : MonoBehaviour {

    [SerializeField] private float _damage = 3f;

    private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<DamageHelper>().TakeDamage(_damage);
        }
	}
}
