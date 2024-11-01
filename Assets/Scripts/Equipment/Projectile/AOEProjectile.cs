using System.Collections;
using UnityEngine;

public class AOEProjectile : Projectile
{
    [Header("AOE Projectile")]

    public int DamageRadius;
    public float Duration;

    public float WaitToTarget = 2;
    public float TargetRadius = 3;

    public GameObject AOEImpact;

    protected bool active;

    protected float firedTime;
    protected Vector3 initTarget;
    protected Vector3 target;

    protected Transform enemyTarget;

    public override void Fire(Vector3 startLoc, Vector3 Direction, ExperienceBase ExperienceBase, int Damage, float Range, float Speed, bool showTrailTracerVisuals = true)
    {
        Fire(startLoc, Direction, ExperienceBase, Damage, Range, Speed, 5, 1, AOEImpact, showTrailTracerVisuals);
    }

    public virtual void Fire(Vector3 startLoc, Vector3 Direction, ExperienceBase ExperienceBase, int Damage, float Range, float Speed, int DamageRadius, 
        float Duration, GameObject AOEImpact = null, bool showTrailTracerVisuals = true)
    {

        this.DamageRadius = DamageRadius;
        this.Duration = Duration;
        if (AOEImpact != null) this.AOEImpact = AOEImpact;

        enemyTarget = null;

        firedTime = Time.time;

        initTarget = GenerateInitTarget(transform.position);
        target = initTarget;

        base.Fire(startLoc, Direction, ExperienceBase, Damage, Range, Speed, showTrailTracerVisuals);

        active = true;
    }

    protected virtual void FixedUpdate()
    {
        if (!active) return;
    
        if (Time.time - firedTime > WaitToTarget && enemyTarget == null)
        {
            SearchForTarget();
        }
        if (enemyTarget != null && enemyTarget.gameObject.activeSelf)
        {
            target = enemyTarget.position;
        }

        if (target == null) return;

        Vector3 dir = target - transform.position;
        float distanceThisFrame = Time.deltaTime * Speed;

        if (dir.magnitude <= distanceThisFrame)
        {
            StartCoroutine(HitTarget(null, transform.position, null));
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    protected override IEnumerator HitTarget(GameObject trailObj, Vector3 hitPoint, IDamageable target)
    {
        active = false;
        PlayHitEffect(hitPoint);
        
        if (AOEImpact != null)
        {
            var impact = AOEImpact.GetComponent<AOEImpact>();
            if (impact != null)
            {
                var aoeObj = ObjectPooler.GetObject(impact.GetName() + "_AOEProjectile", AOEImpact, hitPoint, Quaternion.identity);
                if (aoeObj != null)
                {
                    var aoeScript = aoeObj.GetComponent<AOEImpact>();
                    if (aoeScript != null) aoeScript.TriggerImpact(hitPoint, ExperienceBase, Damage, DamageRadius, Duration);
                }
            }
        }

        this.gameObject.SetActive(false);

        yield return null;
    }

    protected virtual void SearchForTarget()
    {
        var targets = Physics.OverlapSphere(this.transform.position, TargetRadius);
        if (targets.Length > 0)
        {
            float currentDiff = 0;
            float tempDiff = float.MaxValue;
            Transform tempTarget = null;
            foreach (var target in targets)
            {
                if (target.gameObject.GetComponent<IDamageable>() != null &&
                    target.gameObject.GetComponent<Player>() == null)
                {
                    currentDiff = Vector3.Distance(target.transform.position, transform.position);
                    if (currentDiff < tempDiff)
                    {
                        tempDiff = currentDiff;
                        tempTarget = target.transform;
                    }    
                }
            }

            if (tempTarget != null) enemyTarget = tempTarget;
        }
    }

    protected virtual Vector3 GenerateInitTarget(Vector3 startPos)
    {
        return new Vector3(startPos.x, startPos.y, startPos.z + Range);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.GetComponent<IDamageable>() != null &&
            other.gameObject.GetComponent<Player>() == null) ||
            other.gameObject.tag == "Ground")
        {
            StartCoroutine(HitTarget(null, transform.position, null));
        }
    }
}
