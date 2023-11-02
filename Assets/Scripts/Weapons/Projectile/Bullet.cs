using System.Collections;
using UnityEngine;

public class Bullet : Projectile
{
    [Header("Bullet")]

    [SerializeField]
    protected LayerMask mask;
    [SerializeField]
    protected GameObject bulletTrail;
    protected TrailRenderer bulletTrailRenderer;

    protected RaycastHit hit;

    public override void Fire(Vector3 startLoc, bool showTrailTracerVisuals = true)
    {
        base.Fire(startLoc, showTrailTracerVisuals);

        PerformHitCheck(startLoc, showTrailTracerVisuals);
    }

    protected virtual void PerformHitCheck(Vector3 startLoc, bool showTrailTracerVisuals)
    {
        IDamageable script = null;
        GameObject trailObj = null;
        Vector3 hitPoint = startLoc + (Vector3.forward * Range);

        if (Physics.Raycast(new Ray(startLoc, Vector3.forward), out hit, Range, mask))
        {
            script = hit.collider.gameObject.GetComponent<IDamageable>();
            hitPoint = hit.point;
        }

        if (showTrailTracerVisuals) trailObj = ObjectPooler.GetObject(GetName()+"Trail", bulletTrail, startLoc, Quaternion.identity);

        StartCoroutine(HitTarget(trailObj, hitPoint, script));
    }

    protected virtual IEnumerator HitTarget(GameObject trailObj, Vector3 hitPoint, IDamageable target)
    {
        if (trailObj != null)
        {
            var trail = trailObj.GetComponent<TrailRenderer>();
            if (trail != null)
            {
                float time = 0;
                var startPos = trail.transform.position;

                while (time < 1)
                {
                    trail.transform.position = Vector3.Lerp(startPos, hitPoint, time);

                    time += Time.deltaTime / trail.time;

                    yield return null;
                }

                trail.transform.position = hitPoint;

                trailObj.SetActive(false);
            }
        }

        if (target != null)
        {
            PlayHitEffect(hitPoint);
            target.TakeDamage(this);
        }
    }
}
