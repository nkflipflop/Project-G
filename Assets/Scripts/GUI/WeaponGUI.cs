using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponGUI : MonoBehaviour
{
    public TextMeshProUGUI WeaponText;
    public Image WeaponImage;
    public TextMeshProUGUI AmmoText;
    public Image AmmoImage;
    public Slider ReloadSlider;

    [SerializeField] private Image _ammoTextBackground = null;
    [SerializeField] private Image _ammoImageFrame = null;

    public void SetColor(Color32 color)
    {
        _ammoTextBackground.color = color;
        _ammoImageFrame.color = color;
    }

    public void SetReloadSlider(float value)
    {
        ReloadSlider.value = 1 - value;
    }
}