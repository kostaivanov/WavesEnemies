using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsController : MonoBehaviour
{
    internal PlacePlatformHandler placePlatformButton;
    [SerializeField] private GameObject platformPrefab;

    // Start is called before the first frame update
    void Start()
    {
        placePlatformButton = GameObject.FindGameObjectWithTag("PlacePlatform").GetComponent<PlacePlatformHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (placePlatformButton != null)
        {
            if (placePlatformButton.putPlatformClicked)
            {
                //placePlatformButton.putPlatformClicked = false;
                GameObject platform = Instantiate(platformPrefab, this.transform.position, Quaternion.identity);

            }
        }
    }
}
