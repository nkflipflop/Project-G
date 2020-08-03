using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierHandController : HandControllerBase
{

	public EnemyController EnemyController;
    
    public override void SpecialStart() {
        AimDeviation = 2f;
        CurrentWeapon = transform.GetChild(0).GetComponent<WeaponBase>();
		CurrentWeapon.transform.localPosition = WeaponPosition;
    }
    public override void SpecialUpdate() {
        CharacterIsRunning = EnemyController.IsRunning;
    }

    public override void UseWeapon() {
        if (!EnemyController.HealthController.IsDead && 
            EnemyController.IsRunning && 
            EnemyController.DistanceBtwTarget < 3f) {
			CurrentWeapon.Trigger();
		}
		else {
			CurrentWeapon.ReleaseTrigger();
		}
    }
}
