using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class SkillsController : MonoBehaviourPunCallbacks
{
    internal List<PlacePlatformHandler> placePlatformButtons;
    [SerializeField] private List<GameObject> platformPrefab;
    [SerializeField] private LayerMask groundLayer;
    private ContactFilter2D interactFilter;
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
                        SpriteRenderer sprite = platformPrefab[0].GetComponent<SpriteRenderer>();
                        //Collider2D[] otherColliders = Physics2D.OverlapAreaAll(this.gameObject.GetComponent<Collider2D>().bounds.min, collider1.bounds.max, groundLayer);
                        //Collider2D[] otherColliders = Physics2D.OverlapBoxAll(this.gameObject.transform.position, sprite.bounds.size, 0, groundLayer);
                        bool collide = Physics2D.OverlapBox(this.gameObject.transform.position, sprite.bounds.size, 0, groundLayer);
                        List<Collider2D> results = new List<Collider2D>();


                        //interactFilter.SetLayerMask(groundLayer);
                        //platform.GetComponent<SpriteRenderer>().enabled = false;
                        //  if (otherColliders.Length == 0)
                        if (collide == false)
                        {
                            GameObject platform = PhotonNetwork.Instantiate(platformPrefab[0].name, this.transform.position, Quaternion.identity);
                        }
                    }
                    else
                    {
                        BoxCollider2D collider1 = platformPrefab[1].GetComponent<BoxCollider2D>();
                        SpriteRenderer sprite = platformPrefab[1].GetComponent<SpriteRenderer>();
                        bool collide = Physics2D.OverlapBox(this.gameObject.transform.position, sprite.bounds.size, 0, groundLayer);
                        Debug.Log(sprite.bounds.size + " - overlapping = " + collide);

                        if (collide == false)
                        {
                            GameObject platform = PhotonNetwork.Instantiate(platformPrefab[1].name, this.transform.position, Quaternion.identity);
                        }
                    }

                    p.putPlatformClicked = false;
                    
                }
            }
        }
    }
}
