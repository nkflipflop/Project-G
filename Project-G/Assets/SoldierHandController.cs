using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierHandController : HandControllerBase
{

	public EnemyController EnemyController;
    
    public override void SpecialStart(){
        AimDeviation = 2f;
    }
    public override void SpecialUpdate(){
        CharacterIsRunning = EnemyController.IsRunning;
    }

    public override void UseWeapon(){
        if (EnemyController.IsRunning == true && EnemyController.DistanceBtwTarget < 3f){
			CurrentWeapon.Trigger();
		}
		else {
			CurrentWeapon.ReleaseTrigger();
		}
    }
}
