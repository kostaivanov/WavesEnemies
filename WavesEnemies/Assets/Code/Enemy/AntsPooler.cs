using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntsPooler : MonoBehaviour
{
    internal static AntsPooler current;
    [SerializeField] internal List<GameObject> objectsToBePooled;
    [SerializeField] private int pooledAmount;
    [SerializeField] private bool willGrow;
    internal List<GameObject> pooledObjects;
    [SerializeField] internal GameObject parentInstantiateObject;

    // Start is called before the first frame update
    void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            //GameObject obj = Instantiate(pooledObjectsArray[Random.Range(0, pooledObjectsArray.Count)]);
            GameObject obj_1 = Instantiate(objectsToBePooled[0]);
            obj_1.transform.parent = parentInstantiateObject.transform;

            obj_1.SetActive(false);

            pooledObjects.Add(obj_1);
        }

    }

    internal GameObject GetPooledObject(string typeObject)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            string[] prefabsFullName = pooledObjects[i].name.Split(new char[] { '(', ')' }, System.StringSplitOptions.RemoveEmptyEntries);
            string name = prefabsFullName[0];
            Debug.Log("name = " + name);
            if (name == typeObject && !pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        if (willGrow == true)
        {
            //GameObject obj = Instantiate(pooledObjectsArray[Random.Range(0, pooledObjectsArray.Count)]);
            for (int i = 0; i < objectsToBePooled.Count; i++)
            {
                if (typeObject == objectsToBePooled[i].name)
                {
                    GameObject obj = Instantiate(objectsToBePooled[i]);
                    obj.transform.parent = parentInstantiateObject.transform;
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }

        return null;
    }
}
