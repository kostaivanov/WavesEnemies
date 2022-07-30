using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

internal class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Maze maze;
    [SerializeField] private Material closedMaterial;
    [SerializeField] private Material openMaterial;

    List<PathMarker> open = new List<PathMarker>();
    List<PathMarker> closed = new List<PathMarker>();

    [SerializeField] private GameObject start;
    [SerializeField] private GameObject end;
    [SerializeField] private GameObject PathParent;


    [SerializeField] private GameObject goalObject;
    [SerializeField] private GameObject startObject;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] internal SpriteRenderer backGround;

    private PathMarker startNode;
    private PathMarker goalNode;

    private PathMarker lastPosition;
    private bool done = false;
    private bool startMarkerToClosed = false;
    [SerializeField] protected Rigidbody2D rigidBody;
    [SerializeField] private float speed;

    public List<GameObject> waypoints;
    int currentWP = 0;
    public float speedTracker = 1f;
    public float rotationSpeed = 2f;

    public float lookAhead = 1f;
    private GameObject tracker;

    private float direction_X;
    private float direction_Y;

    private bool searching;
    private float autoSpeed = 0.05f;
    private bool f_Pushed;

    // Start is called before the first frame update
    void Start()
    {
        searching = false;
        waypoints = new List<GameObject>();

        tracker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        DestroyImmediate(tracker.GetComponent<Collider2D>());
        tracker.GetComponent<MeshRenderer>().enabled = false;
        tracker.transform.position = startObject.transform.position;
        tracker.transform.rotation = startObject.transform.rotation;
        f_Pushed = false;
    }

    private void RemoveAllMarkers()
    {
        GameObject[] markers = GameObject.FindGameObjectsWithTag("Marker");

        foreach (GameObject marker in markers)
        {
            Destroy(marker);
        }
    }

    private void BeginSearch()
    {
        done = false;
        startMarkerToClosed = false;
        RemoveAllMarkers();

        List<LocationOnTheMap> locations = new List<LocationOnTheMap>();
        for (int y = 1; y < maze.height - 1; y++)
        {
            for (int x = 1; x < maze.width - 1; x++)
            {
                if (maze.map[x, y] != 1)
                {
                    locations.Add(new LocationOnTheMap(x, y));
                }
            }
        }



        Vector3 startLocation = new Vector3(startObject.transform.position.x, startObject.transform.position.y, 0);
        startNode = new PathMarker(new LocationOnTheMap(startObject.transform.position.x, startObject.transform.position.y), 0, 0, 0,
            Instantiate(start, startLocation, transform.rotation * Quaternion.Euler(90f, 0, 0f)), null);

        Vector3 goalLocation = new Vector3(goalObject.transform.position.x, goalObject.transform.position.y, 0);
        goalNode = new PathMarker(new LocationOnTheMap(goalObject.transform.position.x, goalObject.transform.position.y), 0, 0, 0,
            Instantiate(end, goalLocation, transform.rotation * Quaternion.Euler(-90f, 0, 0f)), null);


        open.Clear();
        closed.Clear();
        open.Add(startNode);
        lastPosition = startNode;
        //Debug.Log("last position x = " + lastPosition.location.x + " - last position y = " + lastPosition.location.y);

    }

    private void Search(PathMarker thisNode)
    {
        if (thisNode.Equals(goalNode))
        {
            done = true;
            return; // the goal has been found
        }
        float unityDistance = Vector3.Distance(thisNode.location.ToVector(), goalObject.transform.position);
        //Debug.Log("distance kva e we = " + unityDistance);
        if (unityDistance < 2f)
        {
            done = true;
            return; // the goal has been found
        }

        foreach (LocationOnTheMap dir in maze.directions)
        {
            LocationOnTheMap neighbour = dir + thisNode.location;

            //if (maze.map[(int)neighbour.x, (int)neighbour.y] == 1)
            //{
            //    Debug.Log("opaaaaaaaaaaaaaaaaaaaaaaa");
            //    continue;
            //}

            bool colliders = Physics2D.OverlapBox(new Vector2(neighbour.x, neighbour.y), new Vector3(1, 1, 1), 90, wallLayer);
            //Debug.Log(colliders);
            if (colliders == true)
            {
                //Debug.Log("xaxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                continue;
            }
            //foreach (var pair in maze.wallObjects)
            //{
            //    //if ((float)neighbour.x == pair.Key && (float)neighbour.y == pair.Value)
            //    //{
            //    //    continue;
            //    //}
            //    //if (maze.wallIndexes.Any(k => k.Key == (float)neighbour.x) && maze.wallIndexes.Any(v => v.Key == (float)neighbour.y))
            //    //{
            //    //    continue;
            //    //}

            float leftCorner = backGround.transform.position.x - backGround.bounds.extents.x;
            float rightCorner = backGround.transform.position.x + backGround.bounds.extents.x;

            float topCorner = backGround.transform.position.y + backGround.bounds.extents.y;
            float bottomCorner = backGround.transform.position.y - backGround.bounds.extents.y;

            if (neighbour.x < Mathf.Round(leftCorner) || neighbour.x > Mathf.Round(rightCorner) || neighbour.y < Mathf.Round(bottomCorner) || neighbour.y > Mathf.Round(topCorner))
            {
                //Debug.Log("izlizam ot tuka we");
                continue;
            }
            if (IsClosed(neighbour))
            {
                //Debug.Log("izlizam navun");
                continue;
            }

            float H = Vector2.Distance(neighbour.ToVector(), goalNode.location.ToVector());
            float G = Vector2.Distance(thisNode.location.ToVector(), neighbour.ToVector()) + thisNode.G;
            float F = G + H;

            if (!IfNeighbourExist(neighbour, G, H, F, thisNode))
            {
                GameObject pathBlock = Instantiate(PathParent, new Vector3(neighbour.x, neighbour.y, 0), transform.rotation * Quaternion.Euler(90f, 0, 0f));


                TextMesh[] values = pathBlock.GetComponentsInChildren<TextMesh>();
                values[0].text = "G: " + G.ToString("0.00000");
                values[1].text = "H: " + H.ToString("0.00000");
                values[2].text = "F: " + F.ToString("0.00000");

                if (!UpdateMarker(neighbour, G, H, F, thisNode))
                {
                    open.Add(new PathMarker(neighbour, G, H, F, pathBlock, thisNode));
                }
            }


        }

        if (startMarkerToClosed == false)
        {
            PathMarker startMarker = (PathMarker)open.ElementAt(0);
            open.RemoveAt(0);
            closed.Add(startMarker);

            startMarkerToClosed = true;
        }

        open = open.OrderBy(p => p.F).ToList<PathMarker>();
        PathMarker pm = (PathMarker)open.ElementAt(0);

        closed.Add(pm);
        open.RemoveAt(0);

        pm.marker.GetComponent<Renderer>().material = closedMaterial;

        lastPosition = pm;

        if (lastPosition.location.x < startObject.transform.position.x)
        {
            direction_X = -1f;
        }
        if (lastPosition.location.x > startObject.transform.position.x)
        {
            direction_X = 1f;
        }
        if (lastPosition.location.y < startObject.transform.position.y)
        {
            direction_Y = 1f;
        }
        if (lastPosition.location.y > startObject.transform.position.y)
        {
            direction_Y = -1f;
        }

        //rigidBody.velocity = new Vector2(direction_X * speed, rigidBody.velocity.y);
    }

    private bool UpdateMarker(LocationOnTheMap position, float g, float h, float f, PathMarker parent)
    {
        foreach (PathMarker p in open)
        {
            if (p.location.Equals(position))
            {
                p.G = g;
                p.H = h;
                p.F = f;
                p.parent = parent;

                return true;
            }
        }

        return false;
    }

    private bool IfNeighbourExist(LocationOnTheMap position, float g, float h, float f, PathMarker parent)
    {
        foreach (PathMarker p in open)
        {
            if (p.location.Equals(position))
            {
                p.G = g;
                p.H = h;
                p.F = f;

                return true;
            }
        }

        return false;
    }

    private bool IsClosed(LocationOnTheMap marker)
    {
        foreach (PathMarker p in closed)
        {
            if (p.location.Equals(marker))
            {
                return true;
            }
        }

        return false;
    }
    private void GetPath()
    {
        RemoveAllMarkers();
        PathMarker begin = lastPosition;
        while (!startNode.Equals(begin) && begin != null)
        {
            GameObject pathObject_1 = Instantiate(PathParent, new Vector3(begin.location.x, begin.location.y, 0), transform.rotation * Quaternion.Euler(90f, 0, 0f));
            begin = begin.parent;
            waypoints.Add(pathObject_1);
        }

        GameObject pathObject_2 = Instantiate(PathParent, new Vector3(startNode.location.x, startNode.location.y, 0), transform.rotation * Quaternion.Euler(90f, 0, 0f));
        waypoints.Add(pathObject_2);
        currentWP = waypoints.Count - 1;
    }


    //private Vector3 Cross(Vector3 v, Vector3 w)
    //{
    //    float xMult = v.y * w.z - v.z * w.y;
    //    float yMult = v.z * w.x - v.x * w.z;
    //    float zMult = v.x * w.y - v.y * w.x;

    //    Vector3 crossProd = new Vector3(xMult, yMult, zMult);
    //    return crossProd;
    //}

    //private void CalculateAngle()
    //{
    //    Vector3 pFrwd = startObject.transform.up;
    //    Vector3 rDir = goalObject.transform.position - startObject.transform.position;

    //    float dot = pFrwd.x * rDir.x + pFrwd.y * rDir.y;
    //    float angle = Mathf.Acos(dot / (pFrwd.magnitude * rDir.magnitude));

    //    //Debug.Log("Angle: " + angle * Mathf.Rad2Deg);
    //    //Debug.Log("Unity angle: " + Vector3.Angle(pFrwd, rDir));

    //    //Debug.DrawRay(this.transform.position, pFrwd * 15, Color.green, 2);
    //    //Debug.DrawRay(this.transform.position, rDir, Color.red, 2);

    //    int clockWise = 1;
    //    if (Cross(pFrwd, rDir).z < 0)
    //    {
    //        clockWise = -1;
    //    }

    //    //Unity calculation on the angle
    //    float unityAngle = Vector3.SignedAngle(pFrwd, rDir, startObject.transform.forward);
    //    //Debug.Log("forward: " + this.transform.forward);

    //    startObject.transform.Rotate(0, 0, unityAngle * 0.02f);

    //    //this.transform.Rotate(0, 0, angle * Mathf.Rad2Deg * clockWise);
    //}
    //private void ProgressTracker()
    //{
    //    if (Vector3.Distance(tracker.transform.position, startObject.transform.position) > 1.5f)
    //    {
    //        //float dis = Vector3.Distance(tracker.transform.position, startObject.transform.position);
    //        return;
    //    }

    //    if (Vector3.Distance(tracker.transform.position, waypoints[currentWP].transform.position) < 0.25f)
    //    {
    //        currentWP--;
    //    }

    //    if (currentWP == 0)
    //    {
    //        done = false;
    //        currentWP = waypoints.Count - 1;
    //    }
    //    tracker.transform.LookAt(waypoints[currentWP].transform);
    //    tracker.transform.Translate(0, 0, (speed + 0.5f) * Time.deltaTime);
    //}

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("current waypoint = " + currentWP);
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            BeginSearch();
        }
        if (Input.GetKeyDown(KeyCode.S) && !done)
        {
            searching = true;

        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            foreach (PathMarker item in closed)
            {
                Debug.Log(item.F);
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            foreach (PathMarker item in open)
            {
                Debug.Log(item.F);
            }
        }

        if (searching == true)
        {
            Search(lastPosition);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            GetPath();
        }
        if (Input.GetKeyDown(KeyCode.F) && f_Pushed == false)
        {
            f_Pushed = true;
        }


        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("waypoints bro = " + waypoints.Count);
            foreach (GameObject item in waypoints)
            {
                Debug.Log(item.transform.position);
            }
        }


        if (f_Pushed == true && done == true && waypoints.Count > 0)
        {
            //CalculateAngle();
            //startObject.transform.Translate(startObject.transform.up * autoSpeed, Space.World);
            ////ProgressTracker();
            Debug.Log("bbbb ?");
            Vector3 myLocation = startObject.transform.position;
            Vector3 targetLocation = waypoints[currentWP].transform.position;

            Vector3 direction = (tracker.transform.position - startObject.transform.position);



            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * direction;
            Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);

            startObject.transform.rotation = Quaternion.Slerp(startObject.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            startObject.transform.Translate(speed * Time.deltaTime, 0, 0);

            Debug.DrawRay(startObject.transform.position, direction, Color.red);
            Debug.DrawRay(startObject.transform.position, rotatedVectorToTarget, Color.green);
            Debug.Log("The quaternion -  " + targetRotation);
            Debug.Log("Green Y, Upwards vector " + rotatedVectorToTarget);
            Debug.Log("Direction X, to target vector " + direction);
            Debug.Log("Forward vector " + Vector3.forward);
            Debug.DrawRay(startObject.transform.position, Vector3.up * 5, Color.yellow);
            Debug.DrawRay(startObject.transform.position, Vector3.forward * 5, Color.white);
        }
    }
}
