using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkillsController : MonoBehaviour
{
    internal List<PlacePlatformHandler> placePlatformButtons;
    [SerializeField] private List<GameObject> platformPrefab;

    // Start is called before the first frame update
    void Start()
    {
         placePlatformButtons = new List<PlacePlatformHandler>();
         GameObject.FindGameObjectsWithTag("PlacePlatform").ToList().ForEach(p => placePlatformButtons.Add(p.GetComponent<PlacePlatformHandler>()));
    }

    // Update is called once per frame
    void Update()
    {
        if (placePlatformButtons != null)
        {
            foreach (PlacePlatformHandler p in placePlatformButtons)
            {
                if (p.putPlatformClicked == true)
                {
                    string name = p.gameObject.name;

                    if (name.StartsWith("H"))
                    {
                        GameObject platform = Instantiate(platformPrefab[0], this.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        GameObject platform = Instantiate(platformPrefab[1], this.transform.position, Quaternion.identity);
                    }
                }
            }
        }
    }
}
