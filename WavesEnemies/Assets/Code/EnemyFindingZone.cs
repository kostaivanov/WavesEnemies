using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFindingZone : MonoBehaviour
{
    private EnemyMovement enemy;
    private GameObject antsNest;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<EnemyMovement>();
        antsNest = GameObject.FindGameObjectWithTag("Destination");
    }

    private void OnTriggerEnter2D(Collider2D otherObject)
    {
        if (otherObject.tag == "Player")
        {
            foreach (GameObject obj in enemy.waypoints)
            {
                Destroy(obj);
            }
            enemy.waypoints.Clear();
            Debug.Log("how?");
            enemy.Re_Search(otherObject.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D otherObject)
    {
        if(otherObject.tag == "Player")
        {
            foreach (GameObject obj in enemy.waypoints)
            {
                Destroy(obj);
            }
            enemy.waypoints.Clear();

            enemy.Re_Search(antsNest);
        }
    }
}
