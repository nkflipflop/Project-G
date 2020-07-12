using System;
using UnityEngine;

public class WeaponPrefab : MonoBehaviour {
	public int ID;
	public float ReloadTime;
	public float FireRate;
	public bool HasRecoil;
	public bool Automatic;
	public int MaxAmmo;
	public int BulletPerShot;
	public GameObject Projectile;

}
