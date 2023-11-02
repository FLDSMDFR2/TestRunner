using UnityEngine;

public class ObstacleSpawner : Spawner
{
    public Vector3 LeftSpawnLocation;
    public Vector3 RightSpawnLocation;

    protected override void InitController()
    {
        foreach(var controller in Controllers)
        {
            ObstacleSpawnController cont = controller.Controller as ObstacleSpawnController;
            if (cont != null)
            {
                cont.SetSpawnLocations(LeftSpawnLocation, RightSpawnLocation);
            }
        }
    }
}
