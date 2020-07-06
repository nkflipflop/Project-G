using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public MiscGUI MiscUI;
    public WeaponGUI WeaponUI;


    public void UpdateUI(char whichUI, int value, Sprite sprite){
        if (whichUI == 'w') {
            WeaponUI.WeaponImage.sprite = sprite;
        }
        else if (whichUI == 'a') {
            WeaponUI.AmmoText.text = "" + value;
        }
        else if (whichUI == 'h') {
            MiscUI.HealthText.text = "" + value;
        }
    }
}
