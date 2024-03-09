﻿public class SoldierHandController : HandControllerBase
{
    public EnemyController EnemyController;

    protected override void SpecialStart()
    {
        AimDeviation = 2f;
        CurrentWeapon = transform.GetChild(0).GetComponent<WeaponBase>();
        CurrentWeapon.transform.localPosition = WeaponPosition;
    }

    protected override void SpecialUpdate()
    {
        CharacterIsRunning = EnemyController.IsRunning;
    }

    protected override void UseWeapon()
    {
        if (!EnemyController.HealthController.IsDead &&
            EnemyController.IsRunning &&
            EnemyController.DistanceBtwTarget < 3f)
        {
            CurrentWeapon.Trigger();
        }
        else
        {
            CurrentWeapon.ReleaseTrigger();
        }
    }
}