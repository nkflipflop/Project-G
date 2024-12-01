using Gameplay.Runtime.Controllers;
using UnityEngine;

namespace Gameplay.Runtime.Player
{
    public class PlayerHandController : HandControllerBase
    {
        private Player player;
        private Player Player => player ??= TargetObject.GetComponent<Player>();

        private WeaponBase newWeapon;
        private bool takeWeapon;
        private bool canTake;

        protected override void SpecialStart()
        {
            aimDeviation = 0;
        }

        protected override void SpecialUpdate()
        {
            InteractWithNewWeapon();
            //InterractWithExitDoor();
            characterIsRunning = Player.IsRunning;
        }

        protected override Vector3 GetTargetPosition()
        {
            return GameManager.instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        private void InteractWithNewWeapon()
        {
            if (canTake)
            {
                takeWeapon = Input.GetKeyDown(KeyCode.E);
                if (takeWeapon)
                {
                    DropCurrentWeapon();
                    EquipWeapon(newWeapon);
                }
            }
        }

        private void DropCurrentWeapon()
        {
            currentWeapon.transform.SetParent(null);
            currentWeapon.OnHand(false);
        }
        
        public void EquipWeapon(WeaponBase weapon)
        {
            weapon.transform.SetParent(transform, false);
            weapon.transform.localPosition = weaponPosition;
            weapon.transform.rotation = transform.rotation;

            if (currentWeapon && aimPosition.x - transform.position.x < 0)
            {
                weapon.ScaleInverse();
            }

            currentWeapon = weapon;
            currentWeapon.OnHand(true);
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
}