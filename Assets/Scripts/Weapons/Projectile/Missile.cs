using UnityEngine;

public class Missile : Projectile
{
    [Header("Missile")]

    public int DamageRadius;
    public float Speed;
    public float Height;
    public float WaitToTarget;
    protected float firedTime;

    public GameObject AOEImpact;

    protected bool active;

    protected Vector3 initTarget;
    protected Vector3 target;

    protected Transform enemyTarget;

    public override void Fire(Vector3 startLoc, bool showTrailTracerVisuals = true)
    {
        enemyTarget = null;

        firedTime = Time.time;

        initTarget = GenerateInitTarget(transform.position);
        target = initTarget;

        base.Fire(startLoc, showTrailTracerVisuals);

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
       
        Vector3 dir = target - transform.position;
        float distanceThisFrame = Time.deltaTime * Speed;

        if (dir.magnitude <= distanceThisFrame)
        {
            if (target == initTarget)
            {
                // update for second half of arch
                target = new Vector3(initTarget.x, 0, initTarget.z + Range + 3);
            }
            else
            {
                HitTarget();
                return;
            }
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);

    }

    protected virtual void HitTarget()
    {
        active = false;
        PlayHitEffect(transform.position);
        
        if (AOEImpact != null)
        {
            var impact = AOEImpact.GetComponent<AOEImpact>();
            if (impact != null)
            {
                var aoeObj = ObjectPooler.GetObject(impact.GetName() + "Missile", AOEImpact, transform.position, Quaternion.identity);
                if (aoeObj != null)
                {
                    var aoeScript = aoeObj.GetComponent<AOEImpact>();
                    if (aoeScript != null) aoeScript.TriggerImpact(transform.position);
                }
            }
        }

        this.gameObject.SetActive(false);
    }

    protected virtual void SearchForTarget()
    {
        var targets = Physics.OverlapSphere(this.transform.position, Height + 3);
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
        return new Vector3(startPos.x, startPos.y + Height, startPos.z + Range);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.GetComponent<IDamageable>() != null &&
            other.gameObject.GetComponent<Player>() == null) ||
            other.gameObject.tag == "Ground")
        {
            HitTarget();
        }
    }
}
