using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    private int _health = 5;
    
    //public GameObject deathEffect;
    
    private void Update() {
        if (_health <= 0) {
            //Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    
    public void TakeDamage(int damage){
        _health -= damage;
    }
}
