using UnityEngine;

public class PlayerHandController : HandControllerBase
{
    public PlayerController PlayerController;

    private WeaponBase newWeapon;
    private bool takeWeapon;
    private bool canTake;

    protected override void SpecialStart()
    {
        AimDeviation = 0;
    }

    protected override void SpecialUpdate()
    {
        InteractWithNewWeapon();
        //InterractWithExitDoor();
        CharacterIsRunning = PlayerController.IsRun;
    }

    protected override Vector3 GetTargetPosition()
    {
        return Camera.main.ScreenToWorldPoint(TargetObject.transform.position);
    }

    private void InteractWithNewWeapon()
    {
        if (canTake)
        {
            takeWeapon = Input.GetKeyDown(KeyCode.E);
            //Debug.Log("Çok istiyorsan 'E' ye bas. ;(");
            if (takeWeapon)
            {
                // Dropping the current weapon
                DropCurrentWeapon();
                // Equipping the new weapon
                EquipWeapon(newWeapon);
            }
        }
    }

    // Drops the current weapon
    private void DropCurrentWeapon()
    {
        CurrentWeapon.transform.SetParent(null);
        CurrentWeapon.OnHand(false);
    }

    // Equips the new weapon on ground
    public void EquipWeapon(WeaponBase weapon)
    {
        weapon.transform.SetParent(transform, false);
        weapon.transform.localPosition = WeaponPosition;
        weapon.transform.rotation = transform.rotation;

        if (CurrentWeapon != null && AimPosition.x - transform.position.x < 0)
        {
            weapon.ScaleInverse();
        }

        CurrentWeapon = weapon;
        CurrentWeapon.OnHand(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            canTake = true;
            newWeapon = other.gameObject.GetComponent<WeaponBase>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            canTake = false;
            newWeapon = null;
        }
    }
}