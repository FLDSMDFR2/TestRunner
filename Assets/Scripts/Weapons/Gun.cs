using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float FireRate;

    public int PercentTracerVisuals;

    public Projectile Projectile;

    protected System.Random rnd;

    protected virtual void Start()
    {
        rnd = new System.Random();

        StartCoroutine(Shoot());
    }

    protected virtual IEnumerator Shoot()
    {
        // wait for spawn delay
        yield return new WaitForSeconds(FireRate);
       
        while (GameStateData.IsPaused) yield return null;

        this.Projectile.Fire(transform.position, ShowTracerVisuals());

        StartCoroutine(Shoot());
    }

    protected virtual bool ShowTracerVisuals()
    {
        return rnd.Next(1, 101) <= PercentTracerVisuals;
    }
}
