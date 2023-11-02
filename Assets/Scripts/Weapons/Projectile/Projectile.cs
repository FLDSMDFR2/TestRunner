using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour, IDamager
{
    [Header("Projectile")]
    public string Name;
    public int Damage;
    public float Range;

    public float FirePosOffset;

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
    public virtual void SetDamage(int damage) { }

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

    public virtual void Fire(Vector3 startLoc, bool showTrailTracerVisuals = true)
    {
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
            var hitObj = ObjectPooler.GetObject(GetName()+"Hit", impactEffect, hitPoint, Quaternion.LookRotation(hitPoint));
            if (hitObj != null)
            {
                var hitSystem = hitObj.GetComponent<ParticleSystem>();
                if (hitSystem != null) hitSystem.Play();
            }
        }        
    }
}
