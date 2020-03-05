using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSling : WeaponBase {
    public WeaponSling() {
        reloadTime = 0.1f;
        fireRate = 0.1f;
        damage = 5;
        Debug.Log("Sling Called");
    }
}
