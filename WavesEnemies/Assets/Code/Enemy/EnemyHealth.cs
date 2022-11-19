using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] internal float health;
    private EnemyMovement enemyMovement;

    // Start is called before the first frame update
    void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
    }

    private void OnEnable()
    {
        health = 20;
    }

    private void OnTriggerEnter2D(Collider2D otherObject)
    {
        if (otherObject.tag == "Bullet")
        {
            health -= 10;
            //Debug.Log("how many times hit = " + health);
            Destroy(otherObject.gameObject);
            if (health <= 0)
            {
                this.gameObject.SetActive(false);
                gameObject.transform.position = gameObject.transform.parent.position;
                gameObject.transform.rotation = enemyMovement.startRotation;

                enemyMovement.tracker.transform.position = new Vector3(gameObject.transform.parent.position.x, gameObject.transform.parent.position.y, gameObject.transform.parent.position.z);
                enemyMovement.tracker.transform.rotation = enemyMovement.startRotation;
                foreach (GameObject obj in enemyMovement.waypoints)
                {
                    Destroy(obj);
                }
                enemyMovement.waypoints.Clear();
                return;
            }
        }
    }
}
