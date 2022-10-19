using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAnts : MonoBehaviour
{
    [SerializeField] internal float addToSpawnTime;
    internal float chanceSpawnRare = 0.5f;
    private float initialTimer;
    [SerializeField] private GameObject targetObject;

    private string[] antsNames = new string[] { "EnemyAnt" };

    // Start is called before the first frame update
    void Start()
    {
        initialTimer = addToSpawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(this.gameObject.name + " - position =  " + this.transform.position);
        //if (inGame == null)
        //{
        //    inGame = GameObject.FindGameObjectWithTag("InGame");
        //}

        //if (inGame != null && inGame.activeSelf == true)
        //{
            initialTimer -= Time.deltaTime;
            if (initialTimer <= 0)
            {
                SpawnAnt();
                initialTimer = addToSpawnTime;
            }
        //}
    }

    private void SpawnAnt()
    {
        string typeObject = string.Empty;
        //Debug.Log("name asdsdaas");

        if (Random.Range(0f, 1f) > chanceSpawnRare)
        {
            //typeObject = antsNames[Random.Range(0, antsNames.Length)];
            typeObject = antsNames[0];
            //Debug.Log("random  = " + typeObject);

        }
        GameObject obj = AntsPooler.current.GetPooledObject(typeObject);

        if (obj == null)
        {
            return;
        }
        Debug.Log("obj  == null ");
        obj.transform.SetParent(this.gameObject.transform);
        obj.transform.position = this.transform.position;

        Vector3 direction = (targetObject.transform.position - obj.gameObject.transform.position);
        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * direction;
        Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
        obj.transform.rotation = targetRotation;

        //obj.transform.localRotation = this.transform.rotation;
        obj.SetActive(true);
    }
}
