using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkillsController : MonoBehaviour
{
    internal List<PlacePlatformHandler> placePlatformButtons;
    [SerializeField] private List<GameObject> platformPrefab;
    [SerializeField] private LayerMask groundLayer;

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
                        platform.GetComponent<SpriteRenderer>().enabled = false;
                        bool isTouchingGround = Physics2D.IsTouchingLayers(platform.GetComponent<BoxCollider2D>(), groundLayer);
                        Debug.Log("is touching = " + isTouchingGround);
                        if (!isTouchingGround)
                        {
                            platform.GetComponent<SpriteRenderer>().enabled = true;
                        }
                        else
                        {
                            Destroy(platform);
                        }

                    }
                    else
                    {
                        GameObject platform = Instantiate(platformPrefab[1], this.transform.position, Quaternion.identity);
                        platform.GetComponent<SpriteRenderer>().enabled = false;
                        bool isTouchingGround = Physics2D.IsTouchingLayers(platform.GetComponent<BoxCollider2D>(), groundLayer);

                        if (!isTouchingGround)
                        {
                            platform.GetComponent<SpriteRenderer>().enabled = true;
                        }
                        else
                        {
                            Destroy(platform);
                        }
                    }
                }
            }
        }
    }
}
