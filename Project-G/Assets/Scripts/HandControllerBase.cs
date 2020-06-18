using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandControllerBase : MonoBehaviour
{

	public SpriteRenderer CharacterRenderer;
    public GameObject TargetObject;
    
    protected bool CharacterIsRunning = false;
    protected WeaponBase CurrentWeapon;
    protected Vector3 WeaponPosition = new Vector3 (0, 0, 0);

    private Vector3 _aimPosition;
	private float _verticalTrigger = 1;		// for inverse scaling of the weapon
    


	private void Start() {
		CurrentWeapon = transform.GetChild(0).GetComponent<WeaponBase>();
		CurrentWeapon.transform.localPosition = WeaponPosition;
        SpecialStart();
	}

    public virtual void SpecialStart(){
    }

	private void Update() {
		GetInputs();
		// Controls the weapon
		ControlWeapon();
        SpecialUpdate();
	}

    public virtual void SpecialUpdate(){
    }

	// Adjusts the sorting order of the weapon according to mouse position (player's direction)
	private void AdjustSortingOrder(){
		int sortingOrder = CharacterRenderer.sortingOrder;
		if(CharacterIsRunning == true) {
			Vector2 mouseDir = _aimPosition - transform.parent.position;
			float horizontal = mouseDir.x;
			float vertical = mouseDir.y;
			float slope = horizontal / vertical;
			
			if (-1 < slope && slope < 1 && vertical > 0)	
				sortingOrder -= 4;
			else	
				sortingOrder += 4;
		}
		else {
			sortingOrder += 4;
		}
		
		CurrentWeapon.SetSortingOrder(sortingOrder);
	}

	// Aims the weapon
	private void AimWeapon() {
		// Flipping hand position and weapon direction
		float verticalAxis = _aimPosition.x - transform.position.x;	
		bool scale = _verticalTrigger * verticalAxis > 0 ? false : true;
		_verticalTrigger = verticalAxis;
		if(scale) CurrentWeapon.ScaleInverse();

		// Getting mouse position and direction of player to mouse
		Vector3 aimDirection = (_aimPosition - CurrentWeapon.transform.position).normalized;

		// Rotating the current weapon
		float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
		CurrentWeapon.transform.eulerAngles = new Vector3(0, 0, angle);
	}

	private void ControlWeapon(){
		if(CurrentWeapon != null){
			CurrentWeapon.WeaponUpdate();
			AimWeapon();
			AdjustSortingOrder();
		}
	}

	// Gets Inputs
	private void GetInputs() {
		// Taking aim position
        if(TargetObject){
		    _aimPosition = TargetObject.transform.position;
		    _aimPosition.z = 0f;
	    }
    }
}
