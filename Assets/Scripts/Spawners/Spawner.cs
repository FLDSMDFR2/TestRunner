using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Serializable]
    public class ControllerPercent
    {
        [SerializeField]
        public int PercentChangeToSpawn;
        [SerializeField]
        public SpawnController Controller;
    }

    public float MaxSpawnDelay;
    public float MinSpawnDelay;

    public List<ControllerPercent> Controllers;

    protected float spawnDelayTime = 0;
    protected System.Random rnd;

    protected virtual void Start()
    {
        rnd = new System.Random();

        InitController();

        Controllers = Controllers.OrderBy(x => x.PercentChangeToSpawn).ToList();

        StartCoroutine(CreateSpawnObject());
    }

    protected virtual void InitController(){ }

    protected virtual IEnumerator CreateSpawnObject()
    {
        // wait for spawn delay

        while (GameStateData.IsPaused || GameTimeManager.GetGameSpeed() <= 0) yield return null;

        spawnDelayTime = GetSpawnDelay();
        while (float.IsInfinity(spawnDelayTime))
        {
            spawnDelayTime = GetSpawnDelay();
            yield return null;
        }

        yield return new WaitForSeconds(spawnDelayTime);

        var controllerIndex = GetController();
        if (controllerIndex >= 0)
            Controllers[controllerIndex].Controller.Spawn();
     
        StartCoroutine(CreateSpawnObject());
    }

    protected virtual float GetSpawnDelay()
    {
        var value = rnd.Next(((int)(MinSpawnDelay * 1000)), ((int)(MaxSpawnDelay * 1000)));

        // update delay based on game speed, if we speed up we need to spawn faster
        return (((float)value) / 1000) / GameTimeManager.GetGameSpeed();
    }

    protected virtual int GetController()
    {
        if (Controllers == null || Controllers.Count <= 0) return -1;
        if (Controllers.Count == 0) return 0;

        var percentTotal = Controllers.Sum(x => x.PercentChangeToSpawn);
        if (percentTotal <= 0) return 0;

        var percent = rnd.Next(1, percentTotal + 1);
        var upperBound = 0;
        var lowerBound = 0;
        var retval = -1;

        for (int i = 0; i < Controllers.Count; i++)
        {
            upperBound += Controllers[i].PercentChangeToSpawn;
            if (percent > lowerBound && percent <= upperBound)
            {
                retval = i;
                break;
            }
            lowerBound = upperBound;
        }

        return retval;
    }
}
