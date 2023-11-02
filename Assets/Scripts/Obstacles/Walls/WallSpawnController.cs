using UnityEngine;

public class WallSpawnController : ObstacleSpawnController
{

    protected override void PerformClassLogic(GameObject spawnedObject)
    {
        var wall = spawnedObject.GetComponent<Wall>();
        if (wall == null) return;


        wall.MaxHealth = rnd.Next(wall.MinHealthRange, wall.MaxHealthRange);
    }

    protected override SpawnLocations GetSpawnType()
    {
        return (SpawnLocations)rnd.Next(0, 2);
    }
}
