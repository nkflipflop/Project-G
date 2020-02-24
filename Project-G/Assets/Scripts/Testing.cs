using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{
    [SerializeField] private PlayerAim playerAim;
    
    private void Start() {
        playerAim.OnShoot += PlayerAim_OnShoot;
    }

    private void PlayerAim_OnShoot(object sender, PlayerAim.OnShootEventArgs e){
        UtilsClass.ShakeCamera(1f, .2f);
    }
}
