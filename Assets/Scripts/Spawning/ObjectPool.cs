using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool 
{
    private PoolableObject prefab;
    private List<PoolableObject> availableObjects;
    private static Dictionary<PoolableObject,ObjectPool> objectPools = new Dictionary<PoolableObject, ObjectPool>();
    private GameObject poolObject;

    private ObjectPool(PoolableObject prefab, int size) { 
        this.prefab = prefab;
        availableObjects = new List<PoolableObject>();
        poolObject = new GameObject(prefab.name + " Pool");
    }

    public static ObjectPool CreateInstance(PoolableObject prefab, int size) {
        if(objectPools.ContainsKey(prefab))
        {
            return objectPools[prefab];
        }
        else
        {
            ObjectPool pool = new ObjectPool(prefab, size);

            
            pool.CreateObjects(size);

            objectPools[prefab] = pool;

            return pool;
        }
    }

    private void CreateObjects(int size) {
        for (int i = 0; i < size; i++)
        {
            PoolableObject poolableObject = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity, poolObject.transform);
            poolableObject.parent = this;
            poolableObject.gameObject.SetActive(false);
        }

    }

    public void ReturnObjectToPool(PoolableObject poolableObject) { 
        availableObjects.Add(poolableObject);
        //poolableObject.transform.SetParent(poolObject.transform);
    }


    public PoolableObject GetObject() {
        if (availableObjects.Count == 0)
        {
            //opps we ran out.. make more!
            CreateObjects(10);
        }
        PoolableObject instance = availableObjects[0];
        availableObjects.RemoveAt(0);

        return instance;
    }
}
