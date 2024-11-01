using UnityEngine;

public class AOEAbility : Ability
{
    [Header("AOE Ability")]
    public int BaseDamage;
    public int CurrentDamage;

    public int BaseRadius;
    public int CurrentRadius;

    public float BaseDuration;
    public float CurrentDuration;

    public GameObject AOEImpact;

    protected override void PerformModifierChecks()
    {
        CurrentDamage = BaseDamage;
        CurrentRadius = BaseRadius;
        CurrentDuration = BaseDuration;

        base.PerformModifierChecks();
    }

    protected override void ModifierChecks()
    {

    }
}
