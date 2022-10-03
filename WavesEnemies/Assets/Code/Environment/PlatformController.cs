using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D otherObject)
    {
        if (otherObject.gameObject.tag == "Ground")
        {
            //Destroy(this.gameObject);
           
        }
        //Debug.Log("Detecting = " + otherObject.gameObject.name);
    }
}
