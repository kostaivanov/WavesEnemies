using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AntsPooler : MonoBehaviourPunCallbacks
{
    internal static AntsPooler current;
    [SerializeField] internal List<GameObject> objectsToBePooled;
    [SerializeField] private int pooledAmount;
    [SerializeField] private bool willGrow;
    internal List<GameObject> pooledObjects;
    [SerializeField] private GameObject targetObject;
    //[SerializeField] internal GameObject parentInstantiateObject;

    private void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        pooledObjects = new List<GameObject>();

        for (int i = 0; i < pooledAmount; i++)
        {
            //GameObject obj = Instantiate(pooledObjectsArray[Random.Range(0, pooledObjectsArray.Count)]);
            GameObject obj_1 = PhotonNetwork.Instantiate(objectsToBePooled[0].name, this.gameObject.transform.position, Quaternion.identity);
            //obj_1.transform.parent = parentInstantiateObject.transform;
            //obj_1.transform.position = parentInstantiateObject.transform.position;
            //obj_1.transform.rotation = parentInstantiateObject.transform.rotation;

            obj_1.transform.parent = this.gameObject.transform;
            //obj_1.transform.SetParent(this.gameObject.transform);

            obj_1.transform.position = this.gameObject.transform.position;
            //obj_1.transform.rotation = this.gameObject.transform.rotation;

            //Debug.Log(obj_1.transform.rotation);

            Vector3 direction = (targetObject.transform.position - obj_1.gameObject.transform.position);
            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * direction;
            Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
            obj_1.transform.rotation = targetRotation;

            obj_1.name = obj_1.name + obj_1.transform.GetSiblingIndex();
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
            //Debug.Log(this.gameObject.name + " = name = " + pooledObjects.Count);
            if (name == typeObject && !pooledObjects[i].activeInHierarchy)
            {
                Debug.Log("how many");

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
                    obj.transform.parent = this.gameObject.transform;
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }

        return null;
    }
}
