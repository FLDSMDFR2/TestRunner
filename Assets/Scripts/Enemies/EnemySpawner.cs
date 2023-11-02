
public class EnemySpawner : Spawner
{
    public float DisatnceAway;
    public float MaxX;
    public float MinX;
    public float DistanceFromWalls;

    protected override void InitController()
    {
        foreach (var controller in Controllers)
        {
            EnemySpawnController cont = controller.Controller as EnemySpawnController;
            if (cont != null)
            {
                cont.SetSpawnLocations(MaxX, MinX, DisatnceAway, DistanceFromWalls);
            }
        }
    }
}
