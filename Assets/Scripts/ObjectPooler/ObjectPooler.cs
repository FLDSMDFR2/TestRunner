using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    protected static Dictionary<string, List<GameObject>> objPool;
    protected static Transform myTransform;

    protected virtual void Awake()
    {
        GameEventSystem.Game_Start += GameEventSystem_Game_Start;
    }

    protected virtual void Start()
    {
        myTransform = this.transform;
        objPool = new Dictionary<string, List<GameObject>>();
    }

    protected virtual void GameEventSystem_Game_Start()
    {
        ResetPoolObjs();
    }

    public static GameObject GetObject(string key, GameObject obj, Vector3 pos, Quaternion rot)
    {
        if (objPool.ContainsKey(key) && objPool[key].Count > 0)
        {
            foreach (var item in objPool[key])
            {
                // if this object is not active then it should be available to be used again
                if (!item.activeSelf)
                {
                    item.transform.position = pos;
                    item.transform.rotation = rot;
                    item.SetActive(true);
                    return item;
                }
            }
        }

        // if all objects are currently active create new one
        return CreateObject(key, obj, pos, rot);
    }

    protected static GameObject CreateObject(string key, GameObject obj, Vector3 pos, Quaternion rot)
    {
        // look a parent object to add this new object too
        GameObject retVal = null;
        foreach (Transform child in myTransform)
        {
            if (child.name.Equals(key))
            {
                retVal = Instantiate(obj, pos, rot, child);
                break;
            }
        }

        // if we cant find a parent object create one
        if (retVal == null)
        {
            var parent = new GameObject(key);
            parent.transform.parent = myTransform;

            retVal = Instantiate(obj, pos, rot, parent.transform);
        }

        // if this is the first object of its type create list
        if (!objPool.ContainsKey(key))
        {
            objPool.Add(key, new List<GameObject>());
        }

        // add new object to pool
        objPool[key].Add(retVal);

        return retVal;
    }

    protected virtual void ResetPoolObjs()
    {
        foreach(var key in objPool.Keys)
        {
            foreach(var obj in objPool[key])
            {
                obj.SetActive(false);
            }
        }
    }
}