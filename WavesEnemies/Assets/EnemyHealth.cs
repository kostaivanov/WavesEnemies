using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] internal float health;

    // Start is called before the first frame update
    void Start()
    {
        
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
            Debug.Log("how many times hit = " + health);
            Destroy(otherObject.gameObject);
            if (health <= 0)
            {
                
                this.gameObject.SetActive(false);
                return;
            }
        }
    }
}
