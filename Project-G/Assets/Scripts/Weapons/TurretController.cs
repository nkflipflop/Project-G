using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : HandControllerBase {
	public DamageHelper DamageHelper;
	private bool _targetInRange = false;
	private int _environmentLayer = 9;
	private int _playerLayer = 10;
	private LayerMask _hittableLayersByEnemy;

	public override void SpecialStart(){
        AimDeviation = 2f;
		_hittableLayersByEnemy = (1 << _playerLayer) | (1 << _environmentLayer);
    }

	public override void SpecialUpdate() {
		CheckPlayerInRange();
	}
	
    public override void UseWeapon(){
        if (!DamageHelper.IsDead &&  _targetInRange) {
			CurrentWeapon.Trigger();
		}
		else {
			CurrentWeapon.ReleaseTrigger();
		}
    }

	private void CheckPlayerInRange() {
		if (TargetObject != null) {
			Vector3 direction = TargetObject.transform.position - transform.position;
			direction.y -= .23f;
			RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, direction, 4f, _hittableLayersByEnemy);
			//Debug.DrawRay(transform.position, direction, Color.blue, .1f);
	
			if (hitInfo.collider != null) {
				_targetInRange = hitInfo.collider.tag == "Player";
			}
			else {
				_targetInRange = false;
			}
		}
		else {
			_targetInRange = false;
		}
	}
}
