using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public MiscGUI MiscUI;
    public WeaponGUI WeaponUI;
    public PlayerManager PlayerManager;

    private WeaponBase _weapon = null;  // Current Weapon
    private int _ammo = -1;             // Current Ammo
    private int _health = -1;           // Current Health

    private void Update() {
        if (_weapon != PlayerManager.PlayerHandController.CurrentWeapon) {
            _weapon = PlayerManager.PlayerHandController.CurrentWeapon;
     //       WeaponUI.WeaponImage.sprite = sprite;
        //    WeaponUI.WeaponImage.SetNativeSize();
        }

        if (_ammo != PlayerManager.PlayerHandController.CurrentWeapon.CurrentAmmo) {
            _ammo = PlayerManager.PlayerHandController.CurrentWeapon.CurrentAmmo;
            WeaponUI.AmmoText.text = "" + _ammo;
        }

        if (_health != PlayerManager.HealthController.Health){
            _health = PlayerManager.HealthController.Health;
            MiscUI.HealthText.text = "" + _health;
        }
    }
}
