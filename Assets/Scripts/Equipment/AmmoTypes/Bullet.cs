using UnityEngine;

public class Bullet : Ammo
{
    [Header("Bullet")]
    public int BaseDamage;
    public int CurrentDamage;

    public float BaseRange;
    public float CurrentRange;

    public float BaseSpeed;
    public float CurrentSpeed;


    protected override void PerformModifierChecks()
    {
        CurrentDamage = BaseDamage;
        CurrentRange = BaseRange;
        CurrentSpeed = BaseSpeed;

        base.PerformModifierChecks();
    }

    protected override void ModifierChecks()
    {

    }

    public override void Fire(Vector3 startLoc, Quaternion rotation, Vector3 direction)
    {
        base.Fire(startLoc, rotation, direction);

        var projectile = Projectile.GetComponent<Projectile>();
        if (projectile == null) return;

        var bullet = ObjectPooler.GetObject(projectile.GetName() + "_Bullet", Projectile, startLoc, Quaternion.identity);
        if (bullet != null)
        {
            var projectileScript = bullet.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.Fire(startLoc, direction, this, CurrentDamage, CurrentRange, CurrentSpeed, ShowTracerVisuals());
            }
        }
    }
}
