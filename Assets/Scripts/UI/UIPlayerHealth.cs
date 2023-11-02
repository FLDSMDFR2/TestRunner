
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHealth : MonoBehaviour
{
    public Gradient GradientColors;
    public Image FillImage;

    protected Slider healthSlider;

    protected virtual void Awake()
    {
        healthSlider = GetComponent<Slider>();
        if (healthSlider != null)
        {
            GameEventSystem.Player_HealthUpdate += GameEventSystem_Player_HealthUpdate;
        }
    }

    protected virtual void GameEventSystem_Player_HealthUpdate(Player data)
    {
        healthSlider.minValue = 0;
        healthSlider.maxValue = data.MaxHealth;
        healthSlider.value = data.GetHealth();

        FillImage.color = GradientColors.Evaluate(healthSlider.normalizedValue);
    }
}
