using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierHandController : HandControllerBase
{

	public EnemyController EnemyController;
    
    public override void SpecialUpdate(){
        CharacterIsRunning = EnemyController.IsRun;
    }
}
