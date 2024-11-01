using UnityEngine;

public class Ability : ModifiableEquipment
{
    [Header("Ability")]
    public float CoolDownTimeSec;

    protected float lastActivated = 0f;

    public virtual float CoolDownRemaining()
    {
        if (lastActivated <= 0f) return 1;

        var diff = Time.time - lastActivated;

        return Mathf.Clamp(diff / CoolDownTimeSec, 0, 1);
    }

    public virtual void TriggerAbility()
    {
        PerformModifierChecks();
    }

    public virtual void TriggerAbility(Player player)
    {
        PerformModifierChecks();
    }

    protected virtual bool CanPerformAbility()
    {
        if (CoolDownRemaining() >= 1)
        {
            lastActivated = Time.time;
            return true;
        }

        return false;
    }
}
