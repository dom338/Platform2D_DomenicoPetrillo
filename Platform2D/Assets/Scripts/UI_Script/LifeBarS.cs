using UnityEngine;
using UnityEngine.UI;

public class LifeBarS : MonoBehaviour
{
    public Slider LifeSlider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxHealth(float maxHealth)
    {
        LifeSlider.maxValue = maxHealth;
        LifeSlider.value = maxHealth;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(float health)
    {
        LifeSlider.value = health;

        fill.color = gradient.Evaluate(LifeSlider.normalizedValue);
    }
}
