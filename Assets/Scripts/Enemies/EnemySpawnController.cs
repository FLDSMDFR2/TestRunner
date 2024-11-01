using UnityEngine;

public class EnemySpawnController : SpawnController
{
    protected float distanceAway;
    protected float distanceFromWalls;
    protected float maxX;
    protected float minX;

    protected RaycastHit hit;

    public virtual void SetSpawnLocations(float maxX, float minX, float distanceAway, float distanceFromWalls)
    {
        this.maxX = maxX;
        this.minX = minX;
        this.distanceFromWalls = distanceFromWalls;
        this.distanceAway = distanceAway;
    }
    public override void Spawn() 
    {
        var index = GetSpawnPrefabIndex();
        if (index == -1) return;

        var spawnLoc = new Vector3(GetXlocation(), 0, distanceAway);
        if (!CanSpawn(spawnLoc)) return;

        var enemy = Prefabs[index].Prefab.GetComponentInChildren<Enemy>();
        if (enemy == null) return;

        var enemyObject = ObjectPooler.GetObject(enemy.Name, Prefabs[index].Prefab, spawnLoc, Prefabs[index].Prefab.transform.rotation);
        if (enemyObject != null)
        {
            var script = enemyObject.GetComponentInChildren<ISpawnable>();
            if (script != null)
            {
                script.ResetData();
            }
        }
    }

    protected virtual bool CanSpawn(Vector3 location)
    {
        var hits = Physics.SphereCastAll(location, distanceFromWalls, Vector3.forward);
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.GetComponent<Wall>() != null)
            {
                return false;
            }
        }

        return true;
    }

    protected virtual float GetXlocation() 
    {
        var value = rnd.Next(((int)(minX * 1000)), ((int)(maxX * 1000)));
        return ((float)value) / 1000;
    }
}
