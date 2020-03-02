using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class PlayerAim : MonoBehaviour {

    private Vector3 _mousePosition;
    private Transform _aimTransform;
    private Transform _weaponTransform;

    public Transform shootPoint;
    public GameObject arrow;

    private float _timeBtwShots = 0;
    private float _starTimeBtwShots = 0.15f;
    private SpriteRenderer _renderer;
	
    // TO REACH THE CHILD OBJECT WITH ITS NAME
    private void Awake() {
        _aimTransform = transform.Find("Aim");
        
        _weaponTransform = _aimTransform.Find("Weapon");
        _renderer = _weaponTransform.GetComponent<SpriteRenderer>();
    }

    // UPDATE
    private void Update() {
        GetInputs();
        HandleAiming();
        HandleShooting();
    }

    private void GetInputs(){
        _mousePosition = UtilsClass.GetMouseWorldPosition();
    }

    // AIMS THE WEAPON
    private void HandleAiming() {
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

    // SHOOTS THE WEAPON
    private void HandleShooting(){
        if (_timeBtwShots <= 0){
            if (Input.GetMouseButtonDown(0)){
                // CREATING ARROW
                Instantiate(arrow, shootPoint.position, _aimTransform.rotation);
                _timeBtwShots = _starTimeBtwShots;
            }
        }
        else {
            _timeBtwShots -= Time.deltaTime;
        }
    }
}
