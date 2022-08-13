using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntsPooler : MonoBehaviour
{
    internal static AntsPooler current;
    [SerializeField] internal List<GameObject> pooledObjectsArray;
    [SerializeField] private int pooledAmount;
    [SerializeField] private bool willGrow;
    internal List<GameObject> pooledObjects;
    [SerializeField] internal GameObject firePositionObject, parentInstantiateObject;
    // Start is called before the first frame update
    void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            //GameObject obj = Instantiate(pooledObjectsArray[Random.Range(0, pooledObjectsArray.Count)]);
            GameObject obj_1 = Instantiate(pooledObjectsArray[0]);
            obj_1.transform.parent = parentInstantiateObject.transform;

            obj_1.SetActive(false);

            pooledObjects.Add(obj_1);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal GameObject GetPooledObject(string typeObject)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            string[] prefabsFullName = pooledObjects[i].name.Split(new char[] { '(', ')' }, System.StringSplitOptions.RemoveEmptyEntries);
            string name = prefabsFullName[0];

            if (name == typeObject && !pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        if (willGrow == true)
        {
            //GameObject obj = Instantiate(pooledObjectsArray[Random.Range(0, pooledObjectsArray.Count)]);
            for (int i = 0; i < pooledObjectsArray.Count; i++)
            {
                if (typeObject == pooledObjectsArray[i].name)
                {
                    GameObject obj = Instantiate(pooledObjectsArray[i]);
                    obj.transform.parent = parentInstantiateObject.transform;
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }

        return null;
    }
}
