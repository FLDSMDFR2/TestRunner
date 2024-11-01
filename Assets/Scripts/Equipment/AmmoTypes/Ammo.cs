using UnityEngine;

public class Ammo : ModifiableEquipment
{
    [Header("Ammo")]
    public int PercentTracerVisuals;
    public GameObject Projectile;

    protected System.Random rnd;

    protected virtual void Awake()
    {
        rnd = new System.Random();
    }

    public virtual void Fire(Vector3 startLoc, Quaternion rotation, Vector3 direction)
    {
        PerformModifierChecks();
    }

    protected virtual bool ShowTracerVisuals()
    {
        return rnd.Next(1, 101) <= PercentTracerVisuals;
    }
}
