using UnityEngine;

public class Ability : MonoBehaviour
{
    public Sprite Icon;

    public virtual void TriggerAbility() { }
    public virtual void TriggerAbility(Player player) { }
}
