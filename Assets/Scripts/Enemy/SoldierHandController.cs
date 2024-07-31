using General;

public class SoldierHandController : HandControllerBase
{
    public Enemy enemy;

    protected override void SpecialStart()
    {
        AimDeviation = 2f;
        CurrentWeapon = transform.GetChild(0).GetComponent<WeaponBase>();
        CurrentWeapon.transform.localPosition = WeaponPosition;
    }

    protected override void SpecialUpdate()
    {
        CharacterIsRunning = enemy.IsRunning;
    }

    protected override void UseWeapon()
    {
        if (!(enemy as IHealthInteractable).IsDead && enemy.IsRunning && enemy.distanceBtwTarget < 3f)
        {
            CurrentWeapon.Trigger();
        }
        else
        {
            CurrentWeapon.ReleaseTrigger();
        }
    }
}