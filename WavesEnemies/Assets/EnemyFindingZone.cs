using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFindingZone : MonoBehaviour
{
    private EnemyMovement enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<EnemyMovement>();
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
            enemy.Re_Search();
        }
    }
}
