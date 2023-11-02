using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [Serializable]
    public class PrefabPercent
    {
        [SerializeField]
        public int PercentChangeToSpawn;
        [SerializeField]
        public GameObject Prefab;
    }

    public List<PrefabPercent> Prefabs;

    protected System.Random rnd;
    protected virtual void Start()
    {
        rnd = new System.Random();

        Prefabs = Prefabs.OrderBy(x => x.PercentChangeToSpawn).ToList();
    }

    public virtual void Spawn(){ }

    protected virtual int GetSpawnPrefabIndex()
    {
        if (Prefabs == null || Prefabs.Count <= 0) return -1;
        if (Prefabs.Count == 0) return 0;

        var percentTotal = Prefabs.Sum(x => x.PercentChangeToSpawn);
        if (percentTotal <= 0) return 0;

        var percent = rnd.Next(1, percentTotal+1);
        var upperBound = 0;
        var lowerBound = 0;
        var retval = -1;

        for (int i = 0; i < Prefabs.Count; i++)
        {
            upperBound += Prefabs[i].PercentChangeToSpawn;
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
