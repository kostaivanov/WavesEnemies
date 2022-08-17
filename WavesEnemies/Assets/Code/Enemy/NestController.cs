using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D otherObject)
    {
        if (otherObject.tag == "Enemy")
        {
            otherObject.gameObject.SetActive(false);
            otherObject.gameObject.transform.position = otherObject.gameObject.transform.parent.position;
        }
    }
}
