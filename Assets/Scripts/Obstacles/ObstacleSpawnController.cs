using UnityEngine;

public class ObstacleSpawnController : SpawnController
{
    public enum SpawnLocations
    {
        Left, Right, Both, None
    }

    protected Vector3 leftSpawnLocation;
    protected Vector3 rightSpawnLocation;

    public override void Spawn()
    {
        var index = GetSpawnPrefabIndex();
        if (index == -1) return;

        var type = GetSpawnType();
        GameObject go = null;
        var spawnableObj = Prefabs[index].Prefab.GetComponentInChildren<ISpawnable>();
        if (type == SpawnLocations.Left)
        {
            go = ObjectPooler.GetObject(spawnableObj.GetName(), Prefabs[index].Prefab, leftSpawnLocation, Prefabs[index].Prefab.transform.rotation);
        }
        else if (type == SpawnLocations.Right)
        {
            go = ObjectPooler.GetObject(spawnableObj.GetName(), Prefabs[index].Prefab, rightSpawnLocation, Prefabs[index].Prefab.transform.rotation);
        }

        if (go != null)
        {
            PerformClassLogic(go);

            var script = go.GetComponentInChildren<ISpawnable>();
            if (script != null)
            {
                script.ResetData();
            }
        }
    }

    protected virtual void PerformClassLogic(GameObject spawnedObject){ }

    public virtual void SetSpawnLocations(Vector3 left, Vector3 right) 
    {
        leftSpawnLocation = left;
        rightSpawnLocation = right;
    }

    protected virtual SpawnLocations GetSpawnType()
    {
        return SpawnLocations.None;
    }
}
