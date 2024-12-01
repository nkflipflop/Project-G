using General;

public class SoldierHandController : HandControllerBase
{
    public Enemy enemy;

    protected override void SpecialStart()
    {
        aimDeviation = 2f;
        currentWeapon = transform.GetChild(0).GetComponent<WeaponBase>();
        currentWeapon.transform.localPosition = weaponPosition;

        if (TargetObject == null)
        {   // TODO: this is just temporary fix this code ASAP !!!
            TargetObject = FindObjectOfType<Gameplay.Runtime.Player.Player>().gameObject;
        }
    }

    protected override void SpecialUpdate()
    {
        characterIsRunning = enemy.IsRunning;
    }

    protected override void UseWeapon()
    {
        if (!(enemy as IHealthInteractable).IsDead && enemy.IsRunning && enemy.distanceBtwTarget < 3f)
        {
            currentWeapon.Trigger();
        }
        else
        {
            currentWeapon.ReleaseTrigger();
        }
    }
}