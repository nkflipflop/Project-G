using UnityEngine;
using UnityEngine.UI;

public class BarGUI : MonoBehaviour {

    private Slider _slider;
    public Gradient Gradient;
    public Image Fill1, Fill2;

    private void Start() {
        _slider = gameObject.GetComponent<Slider>();
    }

    public void SetMaxValue(int value) {
        _slider.maxValue = value;
        _slider.value = value;

        Fill1.color = Fill2.color = Gradient.Evaluate(1f);     // max health
    }

    public void SetValue(int value) {
        _slider.value = value;

        Fill1.color = Fill2.color = Gradient.Evaluate(_slider.normalizedValue);
    }
}
