using UnityEngine;

public class MissileAbility : AOEAbility
{ 
    public GameObject Missile;

    [Header("MissileAbility")]
    public float BaseRange;
    public float CurrentRange;

    public float BaseSpeed;
    public float CurrentSpeed;

    protected override void PerformModifierChecks()
    {
        CurrentRange = BaseRange;
        CurrentSpeed = BaseSpeed;

        base.PerformModifierChecks();
    }

    protected override void ModifierChecks()
    {

    }

    public override void TriggerAbility(Player player)
    {
        if (!CanPerformAbility()) return;

        if (Missile == null) return;

        base.TriggerAbility(player);

        var AOEProjectileScript = Missile.GetComponent<AOEProjectile>();
        if (AOEProjectileScript)
        {
            var missileGo = ObjectPooler.GetObject(AOEProjectileScript.GetName() + "_MissileAbility", Missile, 
                player.FireLocation.position, player.FireLocation.transform.rotation);
            if (missileGo != null)
            {
                var spawnScript = missileGo.GetComponent<AOEProjectile>();
                spawnScript.Fire(player.FireLocation.position, Vector3.forward, this, CurrentDamage, CurrentRange, CurrentSpeed, CurrentRadius, CurrentDuration, AOEImpact);
            }
        }
    }
}
