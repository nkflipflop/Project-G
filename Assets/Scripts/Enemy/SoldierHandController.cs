using General;

public class SoldierHandController : HandControllerBase
{
    public Enemy enemy;

    protected override void SpecialStart()
    {
        AimDeviation = 2f;
        CurrentWeapon = transform.GetChild(0).GetComponent<WeaponBase>();
        CurrentWeapon.transform.localPosition = WeaponPosition;

        if (TargetObject == null)
        {   // TODO: this is just temporary fix this code ASAP !!!
            TargetObject = FindObjectOfType<Player.Player>().gameObject;
        }
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