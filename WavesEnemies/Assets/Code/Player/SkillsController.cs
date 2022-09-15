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
                        BoxCollider2D collider1 = platformPrefab[0].GetComponent<BoxCollider2D>();
                        //Collider2D[] otherColliders = Physics2D.OverlapAreaAll(this.gameObject.GetComponent<Collider2D>().bounds.min, collider1.bounds.max, groundLayer);
                        Collider2D[] otherColliders = Physics2D.OverlapBoxAll(this.gameObject.transform.position, collider1.size, 0, groundLayer);

                        //platform.GetComponent<SpriteRenderer>().enabled = false;
                        if (otherColliders.Length == 0)
                        {
                            GameObject platform = Instantiate(platformPrefab[0], this.transform.position, Quaternion.identity);
                        }
                        if (otherColliders.Length > 0)
                        {
                            Debug.Log(otherColliders[0]);
                        }
                    }
                    else
                    {
                        GameObject platform = Instantiate(platformPrefab[1], this.transform.position, Quaternion.identity);
                        platform.GetComponent<SpriteRenderer>().enabled = false;
                        //BoxCollider2D collider1 = platform.GetComponent<BoxCollider2D>();
                        //bool isTouchingGround = Physics2D.IsTouchingLayers(collider1);

                        //if (!isTouchingGround)
                        //{
                        //    platform.GetComponent<SpriteRenderer>().enabled = true;
                        //}
                        //else
                        //{
                        //    Destroy(platform);
                        //}
                    }

                    p.putPlatformClicked = false;
                }
            }
        }
    }
}
