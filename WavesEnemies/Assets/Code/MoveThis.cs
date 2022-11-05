using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveThis : MonoBehaviour
{
    [SerializeField] private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (target.transform.position - this.gameObject.transform.position);

        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * direction;

        Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);

        this.gameObject.transform.localRotation = Quaternion.Slerp(this.gameObject.transform.rotation, targetRotation, 2f * Time.deltaTime);

        this.gameObject.transform.Translate(3 * Time.deltaTime, 0, 0);
    }
}
