using System;
using UnityEngine;

public class CursorController : MonoBehaviour {

    [NonSerialized]
    public WeaponBase CurrentWeapon;

	public Animator Anim;

    void Start() {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update() {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPos;
        
        if (CurrentWeapon != null) {
            // When reloading the weapon
            Anim.SetInteger("CurrentAmmo", CurrentWeapon.CurrentAmmo);

            // When firing, set cursor animation
            if(Input.GetMouseButton(0) && CurrentWeapon.CurrentAmmo > 0)
                Anim.SetTrigger("Clicked");
        }
    }

    // When not firing
    public void GoIdle(){
        Anim.ResetTrigger("Clicked");
    }
}
