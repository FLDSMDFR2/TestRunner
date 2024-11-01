using System.Collections.Generic;
using UnityEngine;

public class CascadingRaycastProjectile : RaycastProjectile
{
    [Header("Cascading Raycast Projectile")]
    public int MaxTargetHitCount;
    protected int targetHitCount;

    protected List<List<GameObject>> targets = new List<List<GameObject>>();

    protected override void ResetData()
    {
        base.ResetData();
        targets.Clear();
        targetHitCount = 0;
    }

    protected override void PerformHitCheck(Vector3 startLoc, Vector3 Direction, bool showTrailTracerVisuals)
    {
        IDamageable script = null;
        GameObject trailObj = null;
        Vector3 hitPoint = startLoc + (Direction * Range);

        if (Physics.Raycast(new Ray(startLoc, Direction), out hit, Range, mask))
        {
            script = hit.collider.gameObject.GetComponent<IDamageable>();
            hitPoint = hit.point;
        }

        if (showTrailTracerVisuals) trailObj = ObjectPooler.GetObject(GetName() + "_Trail", bulletTrail, startLoc, Quaternion.identity);

        StartCoroutine(HitTarget(trailObj, hitPoint, script));

        // if we hit something then perform cascade logic
        if (script != null)
        {
            targets.Add(new List<GameObject>() { hit.collider.gameObject });
            if (targets.Count > targetHitCount) CascadeBullet(targets[targetHitCount]);
        }
    }

    protected virtual void CascadeBullet(List<GameObject> hitObj)
    {
        if (targetHitCount >= MaxTargetHitCount || targets.Count <= targetHitCount || targets[targetHitCount].Count <= 0) return;
        targetHitCount++;
        targets.Add(new List<GameObject>());

        foreach (var target in hitObj)
        {
            var cascadeHits = Physics.OverlapSphere(target.transform.position, 10);
            foreach (var cascadeHit in cascadeHits)
            {
                var script = cascadeHit.gameObject.GetComponent<IDamageable>();
                if (script != null &&
                    cascadeHit.gameObject.GetComponent<Player>() == null &&
                    !HasHit(cascadeHit.gameObject))
                {
                    targets[targetHitCount].Add(cascadeHit.gameObject);
                    var trailObj = ObjectPooler.GetObject(GetName() + "_Trail", bulletTrail, target.transform.position, Quaternion.identity);
                    StartCoroutine(HitTarget(trailObj, cascadeHit.gameObject.transform.position, script));
                }
            }
        }

        if (targets.Count > targetHitCount) CascadeBullet(targets[targetHitCount]);
    }

    protected virtual bool HasHit(GameObject objToCheck)
    {
        foreach(var hitList in targets)
        {
            if (hitList.Contains(objToCheck)) return true;
        }

        return false;
    }
}
