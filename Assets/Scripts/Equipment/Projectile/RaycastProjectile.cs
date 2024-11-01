using UnityEngine;

public class RaycastProjectile : Projectile
{
    [Header("Raycast Projectile")]

    [SerializeField]
    protected LayerMask mask;
    [SerializeField]
    protected GameObject bulletTrail;
    protected TrailRenderer bulletTrailRenderer;

    protected RaycastHit hit;

    public override void Fire(Vector3 startLoc, Vector3 Direction, ExperienceBase ExperienceBase, int Damage, float Range, float Speed, bool showTrailTracerVisuals = true)
    {
        base.Fire(startLoc, Direction, ExperienceBase, Damage, Range, Speed, showTrailTracerVisuals);

        PerformHitCheck(startLoc, Direction, showTrailTracerVisuals);
    }

    protected virtual void PerformHitCheck(Vector3 startLoc, Vector3 Direction, bool showTrailTracerVisuals)
    {
        IDamageable script = null;
        GameObject trailObj = null;
        Vector3 hitPoint = startLoc + (Direction * Range);

        if (Physics.Raycast(new Ray(startLoc, Direction), out hit, Range, mask))
        {
            script = hit.collider.gameObject.GetComponent<IDamageable>();
            hitPoint = hit.point;
        }

        if (showTrailTracerVisuals) trailObj = ObjectPooler.GetObject(GetName()+"_Trail", bulletTrail, startLoc, Quaternion.identity);

        StartCoroutine(HitTarget(trailObj, hitPoint, script));
    }
}
