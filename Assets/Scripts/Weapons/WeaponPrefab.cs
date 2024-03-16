using Pooling;
using UnityEngine;

public class WeaponPrefab : MonoBehaviour
{
    public int ID;
    public string Name;
    public float ReloadTime;
    public float FireRate;
    public bool HasRecoil;
    public bool Automatic;
    public int MaxAmmo;
    public int BulletPerShot;
    public ObjectType BulletType;
    public ProjectileController Bullet;         // TODO: remove this
    public SoundManager.Sound FireSound;
}