using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Gradient color;
    public Slider slider;
    public Image fill, border;

    private void Update()
    {
        border.color = color.Evaluate(slider.value / slider.maxValue);
        fill.color = color.Evaluate(slider.value / slider.maxValue);
    }
    public void SetHealth(float health)
    {
        slider.value = health;
    }

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
}
