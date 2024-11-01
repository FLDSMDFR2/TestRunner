using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIAbility : MonoBehaviour
{
    public Image Selector;
    public Image Icon;
    public UIEquipmentLevel LevelDisplay;
    public Slider CoolDownSlider;

    protected Ability ability;

    protected virtual void OnEnable()
    {
        StartCoroutine(UpdateUIDetails());
    }

    protected virtual void OnDisable()
    {
        this.StopAllCoroutines();
    }

    public virtual void InitUIAbility(Sprite Icon)
    {
        this.Icon.sprite = Icon;
        if (LevelDisplay != null) LevelDisplay.gameObject.SetActive(false);
    }

    public virtual void InitUIAbility(Ability ability)
    {
        this.ability = ability;
        UpdateUIDetails();       
    }

    protected virtual IEnumerator UpdateUIDetails()
    {
        yield return new WaitForSeconds(.3f);

        if (ability != null)
        {
            if (Icon != null) Icon.sprite = ability.Icon;
            if (CoolDownSlider != null) CoolDownSlider.value = ability.CoolDownRemaining();
            if (LevelDisplay != null) LevelDisplay.LevelText.text = "LV." + ability.Level;
        }

        StartCoroutine(UpdateUIDetails());
    }
}
