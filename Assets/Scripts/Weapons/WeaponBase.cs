using System;
using Cysharp.Threading.Tasks;
using Pooling;
using UnityEngine;
using Weapons;
using Type = Weapons.Type;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] private WeaponInfo weaponInfo;
    public WeaponRecoiler WeaponRecoiler;
    public SpriteRenderer WeaponRenderer;
    public SpriteRenderer LeftHandRenderer;
    public SpriteRenderer RightHandRenderer;
    public Transform ShotPoint;
    [NonSerialized] public int CurrentAmmo;

    private float timeBtwShots;
    private bool canTrigger = true;
    private bool isReloading = false;

    public Type WeaponType => weaponInfo.type;
    public string Name => weaponInfo.name;
    public Sprite BulletIcon => weaponInfo.bulletIcon;
    public float ReloadTime => weaponInfo.reloadTime;

    private void Start()
    {
        CurrentAmmo = weaponInfo.ammoCapacity;
    }

    public void OnHand(bool active)
    {
        LeftHandRenderer.gameObject.SetActive(active);
        RightHandRenderer.gameObject.SetActive(active);

        // Default rotation and scale
        if (!active)
        {
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            WeaponRenderer.sortingOrder = -4990;
        }
    }

    // Changes layer of the sprite
    public void SetSortingOrder(int order)
    {
        WeaponRenderer.sortingOrder = order;
        if (LeftHandRenderer)
        {
            LeftHandRenderer.sortingOrder = order + 2;
            RightHandRenderer.sortingOrder = order + 2;
        }
    }

    // Flips the sprite
    public void ScaleInverse()
    {
        Vector3 scale = transform.localScale;
        scale.y *= -1;
        transform.localScale = scale;
    }

    // Fires the Weapon	
    private void Fire()
    {
        CurrentAmmo -= 1;
        timeBtwShots = weaponInfo.fireRate;

        // Fire Effect
        FireEffect fireEffect = PoolFactory.instance.GetObject(ObjectType.FireEffect, ShotPoint.position, ShotPoint.rotation).GameObject
            .GetComponent<FireEffect>();
        fireEffect.Play();
        
        // Recoiling the weapon
        if (weaponInfo.hasRecoil)
        {
            WeaponRecoiler.AddRecoil();
        }

        //Sound effect
        SoundManager.PlaySound(weaponInfo.fireSound, transform.position);

        // Creating bullets
        for (int i = 0; i < weaponInfo.bulletPerShot; i++)
        {
            float angelBtwBullets = 10f;
            float zRotation = ((1 - weaponInfo.bulletPerShot) * angelBtwBullets / 2) + (angelBtwBullets * i);
            Projectile bullet = PoolFactory.instance
                .GetObject(weaponInfo.bulletType, ShotPoint.position,
                    Quaternion.Euler(new Vector3(0, 0, ShotPoint.rotation.eulerAngles.z + zRotation))).GameObject
                .GetComponent<Projectile>();
            bullet.Activate(transform.root.CompareTag("Player"));
        }
    }

    public void Trigger()
    {
        if (canTrigger && timeBtwShots <= 0)
        {
            canTrigger = weaponInfo.automatic;  // if weapon is not automatic, you need to release trigger
            if (CurrentAmmo > 0)
            {
                Fire();
            }
            else
            {
                SoundManager.PlaySound(SoundManager.Sound.NoBullet, transform.position);        // Weapon no bullet sound effect
                canTrigger = false;
            }
        }
    }

    public void ReleaseTrigger()
    {
        canTrigger = true;
    }
    
    public void WeaponUpdate()
    {
        timeBtwShots -= Time.deltaTime;     // For firing

        // Reloading
        if (CurrentAmmo == 0 && !isReloading)
        {
            ReloadWeapon();
        }
    }
    
    private async UniTaskVoid ReloadWeapon()
    {
        isReloading = true;
        await UniTask.Delay(TimeSpan.FromSeconds(weaponInfo.reloadTime));
        CurrentAmmo = weaponInfo.ammoCapacity;
        isReloading = false;
        SoundManager.PlaySound(SoundManager.Sound.Reloaded, transform.position);
    }
}