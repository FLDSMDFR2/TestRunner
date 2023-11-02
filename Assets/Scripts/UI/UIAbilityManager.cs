using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UIAbilityManager : MonoBehaviour
{
    public Color SelectedColor;
    public Color BaseColor;
    public Sprite EmptyAbilityIcon;

    public List<UIAbility> uIAbilities;

    protected int activeIndex;

    protected virtual void Awake()
    {
        uIAbilities = GetComponentsInChildren<UIAbility>().ToList();
        GameEventSystem.Player_ResetData += GameEventSystem_Player_ResetData;
        GameEventSystem.Player_ActiveAbilityUpdate += GameEventSystem_Player_ActiveAbilityUpdate;
    }

    private void GameEventSystem_Player_ActiveAbilityUpdate(Player data)
    {
        DisplayActiveAbility(data.ActiveAbilityIndex);
    }

    private void GameEventSystem_Player_ResetData(Player data)
    {
        for(int i = 0; i < uIAbilities.Count; i++)
        {
            if (data.Abilities != null && data.Abilities.Count > i)
            {
                uIAbilities[i].Icon.sprite = data.Abilities[i].Icon;
            }
            else
            {
                uIAbilities[i].Icon.sprite = EmptyAbilityIcon;
            }
        }
    }

    protected virtual void DisplayActiveAbility(int newIndex)
    {
        uIAbilities[activeIndex].Selector.color = BaseColor;

        if (newIndex < 0) activeIndex = uIAbilities.Count - 1;
        else if (newIndex > uIAbilities.Count - 1) activeIndex = 0;
        else activeIndex = newIndex;

        uIAbilities[activeIndex].Selector.color = SelectedColor;
    }
}
