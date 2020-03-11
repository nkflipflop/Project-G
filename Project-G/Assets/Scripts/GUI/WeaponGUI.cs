using UnityEngine;
using UnityEngine.UI;

public class WeaponGUI : MonoBehaviour {

    public Image WeaponIcon;
    public Image AmmoIcon;
    public Text Text;

    public void FirstSetup(Sprite weaponIcon, Sprite ammoIcon, int ammoCount) {
        WeaponIcon.enabled = true;
        AmmoIcon.enabled = true;
        Text.enabled = true;

        WeaponIcon.sprite = weaponIcon;
        WeaponIcon.SetNativeSize();
        AmmoIcon.sprite = ammoIcon;
        AmmoIcon.SetNativeSize();
        SetAmmoCounter(ammoCount);
    }

    public void SetAmmoCounter(int ammoCount) {
        string text = ammoCount.ToString();
        Text.text = text;
    }

    public void DisableGUI() {
        WeaponIcon.enabled = false;
        AmmoIcon.enabled = false;
        Text.enabled = false;
    }
}
