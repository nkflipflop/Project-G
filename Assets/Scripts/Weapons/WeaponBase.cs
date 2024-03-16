using System;
using Cysharp.Threading.Tasks;
using Pooling;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public WeaponPrefab Weapon;
    public WeaponRecoiler WeaponRecoiler;
    public SpriteRenderer WeaponRenderer;
    public SpriteRenderer LeftHandRenderer;
    public SpriteRenderer RightHandRenderer;
    public Transform ShotPoint;
    [NonSerialized] public int CurrentAmmo;

    private float timeBtwShots;
    private bool canTrigger = true;
    private bool isReloading = false;

    private void Start()
    {
        CurrentAmmo = Weapon.MaxAmmo;
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
        timeBtwShots = Weapon.FireRate;

        // Fire Effect
        FireEffect fireEffect = PoolFactory.instance.GetObject(ObjectType.FireEffect, ShotPoint.position, ShotPoint.rotation).GameObject
            .GetComponent<FireEffect>();
        fireEffect.Play();
        
        // Recoiling the weapon
        if (Weapon.HasRecoil)
        {
            WeaponRecoiler.AddRecoil();
        }

        //Sound effect
        SoundManager.PlaySound(Weapon.FireSound, transform.position);

        // Creating bullets
        for (int i = 0; i < Weapon.BulletPerShot; i++)
        {
            float angelBtwBullets = 10f;
            float zRotation = ((1 - Weapon.BulletPerShot) * angelBtwBullets / 2) + (angelBtwBullets * i);
            ProjectileController bullet = PoolFactory.instance
                .GetObject(Weapon.BulletType, ShotPoint.position,
                    Quaternion.Euler(new Vector3(0, 0, ShotPoint.rotation.eulerAngles.z + zRotation))).GameObject
                .GetComponent<ProjectileController>();
            bullet.Activate(transform.root.CompareTag("Player"));
        }
    }

    public void Trigger()
    {
        if (canTrigger && timeBtwShots <= 0)
        {
            canTrigger = Weapon.Automatic;  // if weapon is not automatic, you need to release trigger
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
        await UniTask.Delay(TimeSpan.FromSeconds(Weapon.ReloadTime));
        CurrentAmmo = Weapon.MaxAmmo;
        isReloading = false;
        SoundManager.PlaySound(SoundManager.Sound.Reloaded, transform.position);
    }
}