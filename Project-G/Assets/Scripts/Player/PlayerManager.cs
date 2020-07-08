using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerController PlayerController;
    public PlayerHandController PlayerHandController;
    public DamageHelper DamageHelper;
    public UIController UIController;
    

    private WeaponBase _weapon = null;  // Current Weapon
    private int _ammo = -1;           // Current Ammo
    private int _health = -1;            // Current Health


    // Update is called once per frame
    private void Update() {
        if (_weapon != PlayerHandController.CurrentWeapon)
            weaponUpdate();

        if (_ammo != _weapon.CurrentAmmo)
            ammoUpdate();

        if (_health != DamageHelper.Health)
            healthUpdate();
    }

    // This only for Read, there is no write operation for player
    private void weaponUpdate(){
        _weapon = PlayerHandController.CurrentWeapon;
        // Trigering UI
        UIController.UpdateUI('w', 0, _weapon.WeaponRenderer.sprite);
    }

    // This only for Read, there is no write operation for player
    private void ammoUpdate(){
        _ammo = _weapon.CurrentAmmo;
        // Trigering UI
        UIController.UpdateUI('a', _ammo, null);
    }

    // This only for Read, there is no write operation for player
    private void healthUpdate(){
        _health = DamageHelper.Health;
        // Trigering UI
        UIController.UpdateUI('h', _health, null);
    }
}
