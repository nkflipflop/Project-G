using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class PlayerAim : MonoBehaviour {

    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs {
        public Vector3 weaponEndPointPosition;
        public Vector3 shootPosition;
    }
    private Transform aimTransform;
    private Transform aimWeaponEndPointTransform;
    private void Awake() {
        aimTransform = transform.Find("Aim");
        aimWeaponEndPointTransform = aimTransform.Find("WeaponEndPoimtPosition");
    }

    private void Update() {
        HandleAiming();
        HandleShooting();
    }

    private void HandleAiming() {
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        if (angle > 90.0f || angle < -90.0f)
            angle += 180f;
        Debug.Log(angle);
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void HandleShooting(){
        if (Input.GetMouseButtonDown(0)){
            Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();

            OnShoot?.Invoke(this, new OnShootEventArgs {
                weaponEndPointPosition = aimWeaponEndPointTransform.position,
                shootPosition = mousePosition,
            });
        }
    }
}
