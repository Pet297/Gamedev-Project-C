using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE: Taken from https://learn.unity.com/tutorial/introduction-to-object-pooling#5ff8d015edbc2a002063971c

public class ObjectPoolScript : MonoBehaviour
{
    //public static ObjectPoolScript SharedInstance;

    static Dictionary<string,List<GameObject>> pooledObjects = new Dictionary<string, List<GameObject>>();

    public string poolInstance;
    public GameObject objectToPool;
    public int amountToPool;

    void Awake()
    {
        //SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!pooledObjects.ContainsKey(poolInstance))
        {
            List<GameObject> pooledObjects0 = new List<GameObject>();
            GameObject tmp;
            for (int i = 0; i < amountToPool; i++)
            {
                tmp = Instantiate(objectToPool);
                tmp.SetActive(false);
                pooledObjects0.Add(tmp);
            }
            pooledObjects.Add(poolInstance, pooledObjects0);
        }
    }

    public GameObject GetPooledObject()
    {
        List<GameObject> pooledObjects0 = pooledObjects[poolInstance];
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects0[i].activeSelf)
            {
                return pooledObjects0[i];
            }
        }
        return null;
    }
}
