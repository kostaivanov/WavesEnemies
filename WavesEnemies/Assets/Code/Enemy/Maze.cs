using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Maze : MonoBehaviour
{
    internal List<LocationOnTheMap> directions = new List<LocationOnTheMap>
    {
        new LocationOnTheMap(1, 0), // right neighbour
        new LocationOnTheMap(0, 1), // up neighbour
        new LocationOnTheMap(-1, 0), // left neighbour
        new LocationOnTheMap(0, -1), // down neighbour
    };

    [SerializeField] internal SpriteRenderer backGround;
    [SerializeField] private PolygonCollider2D backGroundCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject objectChecker;

    //internal Dictionary<byte[,], GameObject> IndexAndGameObject;

    internal List<KeyValuePair<float, float>> wallIndexes;
    internal List<GameObject> wallObjects;
    internal int width;
    internal int height;
    internal byte[,] map;

    Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        wallIndexes = new List<KeyValuePair<float, float>>();
        wallObjects = new List<GameObject>();
        //IndexAndGameObject = new Dictionary<byte[,], GameObject>();
        width = (int)backGround.bounds.size.x;
        height = (int)backGround.bounds.size.y;


        initialPosition = new Vector3(backGround.gameObject.transform.position.x - backGround.bounds.extents.x, backGround.gameObject.transform.position.y - backGround.bounds.extents.y, 0);
        objectChecker.transform.position = initialPosition;

        InitialiseMap();
        MarkTheGround();
        DrawMap();

        //Debug.Log("map width = " + map.GetLength(0));
        //Debug.Log("map height = " + map.GetLength(1));
    }
    void InitialiseMap()
    {
        map = new byte[width, height];
        for (int z = 0; z < height; z++)
            for (int x = 0; x < width; x++)
            {
                map[x, z] = 0;     //1 = wall  0 = corridor
            }
    }

    private void FindMapSpace()
    {
        Collider2D[] overlap = Physics2D.OverlapAreaAll(backGroundCollider.bounds.min, backGroundCollider.bounds.max, groundLayer);
        if (overlap.Length > 1)
        {
            //Debug.Log(string.Format("Found {0} overlapping object(s)", overlap.Length - 1));
        }

        foreach (Collider2D item in overlap)
        {
            //Debug.Log(item.transform.position.x);
        }
    }
    void DrawMap()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[x, y] == 1)
                {
                    Vector3 pos = new Vector3(initialPosition.x + x, initialPosition.y + y, 0);
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);

                    wall.transform.localScale = new Vector3(1, 1, 1);
                    wall.transform.position = pos;
                    wall.layer = 6;
                    ////IndexAndGameObject.Add(new byte[x, y]);
                    //Debug.Log("x = " + pos.x + " - y = " + pos.y + " " + (wall.layer.ToString()));
                    wallIndexes.Add(new KeyValuePair<float, float>(pos.x, pos.y));
                    wallObjects.Add(wall);
                }
            }
        }
    }

    public virtual void MarkTheGround()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 pos = new Vector3(initialPosition.x + x, initialPosition.y + y, 0);
                objectChecker.transform.position = pos;
                bool colliders = Physics2D.OverlapBox(objectChecker.transform.position, new Vector3(0.5f, 0.5f, 0), 90, groundLayer);
                //Debug.Log("marking the ground");

                if (colliders)
                {
                    map[x, y] = 1;
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FindMapSpace();
        }
    }
}
