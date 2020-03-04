using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour {
    protected float reloadTime;
    protected float fireRate;
    protected float damage;

    private float _timeBtwShots;

    public WeaponBase() {
        reloadTime = 1;
        fireRate = 1;
        damage = 1;
        Debug.Log("Weapon Constructor");
    }
    public void Shoot(GameObject arrow, Transform aimTransform, Transform shootPoint){
        if (_timeBtwShots <= 0){
            if (Input.GetMouseButtonDown(0)){
                // CREATING ARROW
                Instantiate(arrow, shootPoint.position, aimTransform.rotation);
                Debug.Log("Damage: " + damage);
                _timeBtwShots = fireRate;
            }
        }
        else {
            _timeBtwShots -= Time.deltaTime;
        }
    }
}
