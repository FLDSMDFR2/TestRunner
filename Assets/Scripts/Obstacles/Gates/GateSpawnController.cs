public class GateSpawnController : ObstacleSpawnController
{
    protected override SpawnLocations GetSpawnType()
    {
        return (SpawnLocations)rnd.Next(0, 3);
    }
}
