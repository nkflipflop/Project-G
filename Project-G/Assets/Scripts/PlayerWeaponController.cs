using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour {

    private Vector3 _mousePosition;
    private Transform _aimTransform;

    public Transform shootPoint;
    public GameObject arrow;

    private float _timeBtwShots = 0;
    private float _starTimeBtwShots = 0.15f;
    
    private SpriteRenderer _renderer;
	

    private void Awake() {
        // Reaching the child with its name
        _aimTransform = transform.Find("Hand");
        _renderer = _aimTransform.Find("Weapon").gameObject.GetComponent<SpriteRenderer>();
    }

    // Update
    private void Update() {
        GetInputs();
        AimWeapon();
        ShootWeapon();
    }

    private void GetInputs(){
        // Taking mouse position
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePosition.z = 0f;
    }

    // Aims the weapon
    private void AimWeapon() {
        // GETTING MOUSE POSITION AND DIRECTION OF PLAYER TO MOUSE
        Vector3 aimDirection = (_mousePosition - transform.position).normalized;

		// Flipping the sprite vertically with respect to mouse
		float direction = _mousePosition.x - transform.position.x;
		if (direction < 0){
            _aimTransform.localPosition = new Vector3(Mathf.Abs(_aimTransform.localPosition.x), _aimTransform.localPosition.y, 0f);
        	_renderer.flipY = true;
        }
		else {
            _aimTransform.localPosition = new Vector3(-Mathf.Abs(_aimTransform.localPosition.x), _aimTransform.localPosition.y, 0f);
			_renderer.flipY = false;
        }
        // CONVERTING EULER ANGLE
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        _aimTransform.eulerAngles = new Vector3(0, 0, angle);
    }

    // Shoots the weapon
    private void ShootWeapon(){
      //  _weapon.Shoot(arrow, _aimTransform, shootPoint);
    }
}
