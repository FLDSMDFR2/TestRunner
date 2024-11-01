using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun")]
    public float RateOfFire;
    public Ammo Projectile;

    #region Fire Modifier
    [Header("Fire Modifier")]
    /// <summary>
    /// Amount of projectiles to shoot
    /// </summary>
    public int ProjectileAmount = 1;
    /// <summary>
    /// Arch used for setting start / end angles
    /// </summary>
    public float Arch;
    /// <summary>
    /// starting arch off set
    /// </summary>
    public float ArchOffset;
    /// <summary>
    /// If we want a random spread within the weapon arch
    /// </summary>
    public bool RandomSpread;
    /// <summary>
    /// Start angle of shoot spread
    /// </summary>
    protected float StartAngle = 180;
    /// <summary>
    /// End angle of shoot spread
    /// </summary>
    protected float EndAngle = 180;
    /// <summary>
    /// Number of burst to perform
    /// </summary>
    public int BurstAmount = 1;
    /// <summary>
    /// time between each burst shot
    /// </summary>
    public float BurstRate = 0.5f;
    /// <summary>
    /// If we are currently firing
    /// </summary>
    protected bool isFiring = false;
    public bool IsFiring { get { return isFiring; } }
    /// <summary>
    /// Last time we fired
    /// </summary>
    protected float lastFire = 0f;
    /// <summary>
    /// position to fire from
    /// </summary>
    protected Transform firePos;

    protected IPoolable projectialPoolObject;
    #endregion

    protected virtual void Start()
    {
        StartCoroutine(Shoot());
    }

    protected virtual IEnumerator Shoot()
    {
        // wait for spawn delay
        yield return new WaitForSeconds(RateOfFire);
       
        while (GameStateData.IsPaused || GameStateData.IsGameOver) yield return null;

        // if last fire is 0 then fire this is the first shot
        // or fire when rate allows && we are currently not firing
        if (!isFiring && (lastFire <= 0f || Time.time >= lastFire + RateOfFire))
        {
            isFiring = true;
            StartCoroutine(FireProjectial());
        }

        StartCoroutine(Shoot());
    }

    #region Fire Logic
    /// <summary>
    /// Create and Fire funcunality for this modifer
    /// </summary>
    protected virtual IEnumerator FireProjectial()
    {
        StartAngle = -(Arch / 2) + ArchOffset;
        EndAngle = (Arch / 2) + ArchOffset;

        if (BurstAmount <= 0) BurstAmount = 1;
        var burstCount = 0;
        // perform burst fire if needed
        while (burstCount < BurstAmount)
        {

            if (RandomSpread)
            {
                CreateRandomSpread();
            }
            else
            {
                CreateEvenSpread();
            }

            burstCount++;
            if (BurstAmount > 1) yield return new WaitForSeconds(BurstRate);
        }

        // firing is complete
        isFiring = false;
        lastFire = Time.time;
    }

    protected virtual void CreateEvenSpread()
    {
        // fire projectile pattern
        var pa = ProjectileAmount - 1;
        if (pa <= 0) pa = 1;

        float angleStep = (EndAngle - StartAngle) / pa;
        float angle = this.transform.eulerAngles.y + StartAngle;

        for (int i = 0; i < ProjectileAmount; i++)
        {
            Vector3 dir = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0F, Mathf.Cos(Mathf.Deg2Rad * angle));
            Quaternion rot = Quaternion.Euler(this.transform.rotation.eulerAngles.x, angle, this.transform.rotation.eulerAngles.z);

            if (Projectile != null)
            {
                Projectile.Fire(transform.position, rot, dir);
            }

            angle += angleStep;
        }
    }

    protected virtual void CreateRandomSpread()
    {
        // fire projectile pattern
        var pa = ProjectileAmount - 1;
        if (pa <= 0) pa = 1;

        var angleStart = this.transform.eulerAngles.y + StartAngle;
        var angleEnd = this.transform.eulerAngles.y + EndAngle;

        for (int i = 0; i < ProjectileAmount; i++)
        {
            float angle = RandomGenerator.UnseededRange(angleStart, angleEnd);

            Vector3 dir = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0F, Mathf.Cos(Mathf.Deg2Rad * angle));
            Quaternion rot = Quaternion.Euler(this.transform.rotation.eulerAngles.x, angle, this.transform.rotation.eulerAngles.z);

            if (Projectile != null)
            {
                Projectile.Fire(transform.position, rot, dir);
            }
        }
    }

    #endregion
}
