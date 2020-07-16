using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiscGUI : MonoBehaviour {
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI MedkitText;
    public TextMeshProUGUI ShieldText;
    public GameObject KeyUI;

    public Color maxHealthColor;
    public Color minHealthColor;

    // Color adjustment for Medkit
    [SerializeField] private Image _medkitTextBackground;
    [SerializeField] private Image _medkitImageFrame;

    // Color adjustment for Shield
    [SerializeField] private Image _shieldTextBackground;
    [SerializeField] private Image _shieldImageFrame;

    // Color adjustment for Medkit
    public void SetMedkitColor(Color32 color){
        _medkitTextBackground.color = color;
        _medkitImageFrame.color = color;
    }

    // Color adjustment for Shield
    public void SetShieldColor(Color32 color){
        _shieldTextBackground.color = color;
        _shieldImageFrame.color = color;
    }


    [SerializeField] private Image _healthTextBackground;
 
    public void SetHealthColor(float weight) {
        _healthTextBackground.color = maxHealthColor * weight + minHealthColor * (1 - weight);
    }
}