using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour, IDamager
{
    [Header("Projectile")]
    public string Name;

    public int Damage;
    public float Range;
    public float Speed;

    public float FirePosOffset;

    public ExperienceBase ExperienceBase;

    [SerializeField]
    protected GameObject muzzelFlash;
    protected ParticleSystem muzzelFlashSystem;
    [SerializeField]
    protected GameObject impactEffect;

    protected virtual void Awake()
    {
        if (muzzelFlash != null) 
            muzzelFlashSystem = muzzelFlash.GetComponent<ParticleSystem>();
    }

    #region IDamager
    public virtual void SetDamage(int damage) 
    { 
        Damage = damage;
    }

    public virtual int GetDamage()
    {
        return Damage;
    }

    public virtual string GetName()
    {
        return Name;
    }

    public virtual string GetDtls()
    {
        return "Projectile Dtls";
    }
    #endregion

    public virtual void Fire(Vector3 startLoc, Vector3 Direction, ExperienceBase ExperienceBase, int Damage, float Range, float Speed, bool showTrailTracerVisuals = true)
    {
        this.Damage = Damage;
        this.Range = Range;
        this.Speed = Speed;
        this.ExperienceBase = ExperienceBase;

        ResetData();

        startLoc += (FirePosOffset * Vector3.forward);
        if (muzzelFlash != null) muzzelFlash.transform.position = startLoc;

        PlayFireEffect();
    }

    protected virtual void ResetData() { }

    protected virtual void PlayFireEffect()
    {
        if (!muzzelFlash.activeSelf) muzzelFlash.SetActive(true);
        if (muzzelFlashSystem != null) muzzelFlashSystem.Play();
    }

    protected virtual void PlayHitEffect(Vector3 hitPoint)
    {
        if (impactEffect != null)
        {
            var hitObj = ObjectPooler.GetObject(GetName()+"_Hit", impactEffect, hitPoint, Quaternion.LookRotation(hitPoint));
            if (hitObj != null)
            {
                var hitSystem = hitObj.GetComponent<ParticleSystem>();
                if (hitSystem != null) hitSystem.Play();
            }
        }        
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
            target.TakeDamage(this, ExperienceBase, null);
        }

        this.gameObject.SetActive(false);
    }
}
