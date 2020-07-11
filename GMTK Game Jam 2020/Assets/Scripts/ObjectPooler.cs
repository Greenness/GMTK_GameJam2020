using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  https://www.raywenderlich.com/847-object-pooling-in-unity#:~:text=Object%20pooling%20is%20where%20you,objects%20from%20a%20%E2%80%9Cpool%E2%80%9D.
/// </summary>
public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;
    public GameObject objectToPool;
    public List<GameObject> pooledObjects;
    public int startAmountToPool;
    public ObjectPooler(GameObject objToPool)
    {
        objectToPool = objToPool;
        SharedInstance = this;
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < startAmountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SharedInstance = this;
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < startAmountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        GameObject obj = (GameObject)Instantiate(objectToPool);
        obj.SetActive(false);
        pooledObjects.Add(obj);
        return obj;
    }
}
