using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

internal class EnemyMovement : MonoBehaviour
{
    private Maze maze;
    [SerializeField] private Material closedMaterial;
    [SerializeField] private Material openMaterial;

    List<PathMarker> open = new List<PathMarker>();
    List<PathMarker> closed = new List<PathMarker>();

    //private GameObject start;
    private GameObject end;
    private GameObject pathObjectsParent;
    [SerializeField] private GameObject PathParent, emptyBlock;

    //[SerializeField] private GameObject goalObject;
    //[SerializeField] private GameObject startObject;
    [SerializeField] private LayerMask wallLayer, pathLayer;
    internal SpriteRenderer backGround;
    internal Camera mainCamera;
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;

    private PathMarker startNode;
    private PathMarker goalNode;

    private PathMarker lastPosition;
    internal bool done = false;
    private bool startMarkerToClosed = false;
    //[SerializeField] protected Rigidbody2D rigidBody;
    [SerializeField] private float speed;

    internal List<GameObject> waypoints;
    int currentWP;
    //public float speedTracker = 1f;
    [SerializeField] private float rotationSpeed = 2f;

    //public float lookAhead = 1f;
    [SerializeField] internal GameObject trackerPrefab;
    internal GameObject tracker;
    private float direction_X;
    private float direction_Y;

    internal bool searching = false;
    private float autoSpeed = 0.05f;
    private bool f_Pushed;
    
    Collider[] hitColliders;
    internal Quaternion startRotation;
    LocationOnTheMap neighbour_1;
    private List<GameObject> groundObjects;
    private List<Collider2D> mapGround;
    internal List<PlacePlatformHandler> placePlatformButtons;

    private void Awake()
    {
        maze = GameObject.FindGameObjectWithTag("Maze").GetComponent<Maze>();
        end = GameObject.FindGameObjectWithTag("Destination");
        //backGround = GameObject.FindGameObjectWithTag("Background").GetComponent<SpriteRenderer>();
        //Debug.Log("maze = " + maze == null);
        //Debug.Log("end = " + end == null);
        //Debug.Log("backGround = " + backGround == null);
        mapGround = new List<Collider2D>();
        groundObjects = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        objectHeight = mainCamera.orthographicSize;
        objectWidth = objectHeight * mainCamera.aspect;

        //objectWidth = mainCamera.transform.GetComponent<SpriteRenderer>().bounds.extents.x; //extents = size of width / 2
        //objectHeight = mainCamera.transform.GetComponent<SpriteRenderer>().bounds.extents.y; //extents = size of height / 2
        //Debug.Log("width = " + objectWidth + " - height = " + objectHeight);
        startRotation = this.gameObject.transform.rotation;
        //index = this.transform.GetSiblingIndex();
        waypoints = new List<GameObject>();

        //tracker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        tracker = Instantiate(trackerPrefab);
        //DestroyImmediate(tracker.GetComponent<Collider2D>());
        //tracker.GetComponent<MeshRenderer>().enabled = false;
        tracker.transform.position = this.gameObject.transform.position;
        tracker.transform.rotation = this.gameObject.transform.rotation;
        tracker.name = tracker.name + this.gameObject.transform.GetSiblingIndex();
        f_Pushed = false;
        GameObject.FindGameObjectsWithTag("Ground").ToList().ForEach(o => mapGround.Add(o.GetComponent<Collider2D>()));
        placePlatformButtons = new List<PlacePlatformHandler>();
        GameObject.FindGameObjectsWithTag("PlacePlatform").ToList().ForEach(p => placePlatformButtons.Add(p.GetComponent<PlacePlatformHandler>()));
        //Debug.Log("placePlatformButton found = " + placePlatformButton == null);
        //foreach (GameObject obj in groundObjects)
        //{
        //    mapGround.Add(obj.GetComponent<Collider2D>());
        //}
    }

    private void OnEnable()
    {
        BeginSearch();

        if (!done)
        {
            searching = true;
        }
        if (tracker != null)
        {
            tracker.transform.position = this.transform.position;
        }
    }

    private void OnDisable()
    {
        this.gameObject.transform.rotation = startRotation;

        if (pathObjectsParent != null && pathObjectsParent.transform.childCount > 0)
        {
            RemoveMarkers(pathObjectsParent);
        }
    }

    private void RemoveAllMarkers()
    {
        GameObject[] markers = GameObject.FindGameObjectsWithTag("Marker");

        foreach (GameObject marker in markers)
        {
            Destroy(marker.gameObject);
        }
    }
    private void RemoveMarkers(GameObject obj)
    {
        //GameObject[] markers = GameObject.FindGameObjectsWithTag("Marker");

        foreach (Transform marker in obj.transform)
        {
            Destroy(marker.gameObject);
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

        Vector3 startLocation = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0);
        startNode = new PathMarker(new LocationOnTheMap(this.gameObject.transform.position.x, this.gameObject.transform.position.y), 0, 0, 0,
            this.gameObject, null);

        Vector3 goalLocation = new Vector3(end.transform.position.x, end.transform.position.y, 0);
        goalNode = new PathMarker(new LocationOnTheMap(end.transform.position.x, end.transform.position.y), 0, 0, 0,
            end, null);

        //Vector3 startLocation = new Vector3(startObject.transform.position.x, startObject.transform.position.y, 0);
        //startNode = new PathMarker(new LocationOnTheMap(startObject.transform.position.x, startObject.transform.position.y), 0, 0, 0,
        //    Instantiate(start, startLocation, transform.rotation * Quaternion.Euler(90f, 0, 0f)), null);

        //Vector3 goalLocation = new Vector3(goalObject.transform.position.x, goalObject.transform.position.y, 0);
        //goalNode = new PathMarker(new LocationOnTheMap(goalObject.transform.position.x, goalObject.transform.position.y), 0, 0, 0,
        //    Instantiate(end, goalLocation, transform.rotation * Quaternion.Euler(-90f, 0, 0f)), null);


        open.Clear();
        closed.Clear();
        open.Add(startNode);
        lastPosition = startNode;
        //Debug.Log("how many times begin search is played = ");

    }

    private void Search(PathMarker thisNode)
    {
        //if (thisNode.Equals(goalNode))
        //{
        //    done = true;
        //    return; // the goal has been found
        //}

        ///float unityDistance = Vector3.Distance(thisNode.location.ToVector(), goalObject.transform.position);
        float unityDistance = Vector3.Distance(thisNode.location.ToVector(), end.transform.position);
        //Debug.Log("distance kva e we = " + unityDistance);
        if (unityDistance < 1f)
        {
            done = true;
            searching = false;
            return; // the goal has been found
        }

        foreach (LocationOnTheMap dir in maze.directions)
        {
            LocationOnTheMap neighbour = dir + thisNode.location;
            //Debug.Log("x = " + neighbour.x + " , y = " + neighbour.y);
            //if (maze.map[(int)neighbour.x, (int)neighbour.y] == 1)
            //{
            //    Debug.Log("opaaaaaaaaaaaaaaaaaaaaaaa");
            //    continue;
            //}
            neighbour_1 = neighbour;
            bool ifCollidesWall = Physics2D.OverlapBox(new Vector2(neighbour.x, neighbour.y), new Vector3(1, 1, 1), 90, wallLayer);
            bool ifCollidesPath = Physics2D.OverlapBox(new Vector2(neighbour.x, neighbour.y), new Vector3(1, 1, 1), 90, pathLayer);
            //Debug.Log(colliders);
            if (ifCollidesWall == true || ifCollidesPath)
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

            //float leftCorner = backGround.transform.position.x - backGround.bounds.extents.x;
            //float rightCorner = backGround.transform.position.x + backGround.bounds.extents.x;

            //float topCorner = backGround.transform.position.y + backGround.bounds.extents.y;
            //float bottomCorner = backGround.transform.position.y - backGround.bounds.extents.y;

            float leftCorner = mainCamera.transform.position.x - objectWidth;
            float rightCorner = mainCamera.transform.position.x + objectWidth;
            //Debug.Log($"leftCorner = {mainCamera.transform.position.x} - {objectWidth} = {mainCamera.transform.position.x - objectWidth}");
            float topCorner = mainCamera.transform.position.y + objectHeight;
            float bottomCorner = mainCamera.transform.position.y - objectHeight;
            

            //Debug.Log("leftCorner = " + leftCorner + "; rightCorner = " + rightCorner + "; topCorner = " + topCorner + "; bottomCorner = " + bottomCorner);

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
            hitColliders = Physics.OverlapSphere(neighbour.ToVector(), 0.2f, pathLayer);

            if (!IfNeighbourExist(neighbour, G, H, F, thisNode) || !CheckIfExistInClosedList(neighbour, G, H, F, thisNode))
            {
                //GameObject pathBlock = Instantiate(PathParent, new Vector3(neighbour.x, neighbour.y, 0), transform.rotation * Quaternion.Euler(0f, 0f, 0f));
                if (hitColliders.Length == 0)
                {
                    GameObject pathBlock = Instantiate(PathParent, new Vector3(neighbour.x, neighbour.y, 0), Quaternion.identity);

                    TextMesh[] values = pathBlock.GetComponentsInChildren<TextMesh>();
                    values[0].text = "G: " + G.ToString("0.0");
                    values[1].text = "H: " + H.ToString("0.0");
                    values[2].text = "F: " + F.ToString("0.0");

                    if (!UpdateMarker(neighbour, G, H, F, thisNode))
                    {
                        open.Add(new PathMarker(neighbour, G, H, F, pathBlock, thisNode));
                    }
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

        //if (lastPosition.location.x < startObject.transform.position.x)
        //{
        //    direction_X = -1f;
        //}
        //if (lastPosition.location.x > startObject.transform.position.x)
        //{
        //    direction_X = 1f;
        //}
        //if (lastPosition.location.y < startObject.transform.position.y)
        //{
        //    direction_Y = 1f;
        //}
        //if (lastPosition.location.y > startObject.transform.position.y)
        //{
        //    direction_Y = -1f;
        //}

        //rigidBody.velocity = new Vector2(direction_X * speed, rigidBody.velocity.y);
    }

    private bool CheckIfExistInClosedList(LocationOnTheMap position, float g, float h, float f, PathMarker parent)
    {
        foreach (PathMarker p in closed)
        {
            if (p.location.Equals(position) || p.H == h)
            {
                return true;
            }
        }
        return false;
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
        //foreach (var p in open.Zip(closed, (a, b) => new { A = a, B = b }))
        foreach (var p in open)
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
        if (pathObjectsParent == null)
        {
            pathObjectsParent = new GameObject();
            pathObjectsParent.transform.parent = maze.gameObject.transform;
            pathObjectsParent.name = this.gameObject.name;
        }


        RemoveAllMarkers();
    
        PathMarker begin = lastPosition;
        while (!startNode.Equals(begin) && begin != null)
        {
            GameObject pathObject = Instantiate(emptyBlock, new Vector3(begin.location.x, begin.location.y, 0), transform.rotation * Quaternion.Euler(90f, 0, 0f));

            pathObject.transform.parent = pathObjectsParent.gameObject.transform;
            begin = begin.parent;
            waypoints.Add(pathObject);
            //Debug.Log(pathObject_1.name + " position = " + pathObject_1.transform.position + "begin - " + begin.location.ToVector() + "startnode - " + startNode.location.ToVector());
        }
        currentWP = waypoints.Count - 1;
        //GameObject pathObject_2 = Instantiate(PathParent, new Vector3(startNode.location.x, startNode.location.y, 0), transform.rotation * Quaternion.Euler(90f, 0, 0f));
        //waypoints.Add(pathObject_2);
        //currentWP = waypoints.Count - 1;
    }


    private Vector3 Cross(Vector3 v, Vector3 w)
    {
        float xMult = v.y * w.z - v.z * w.y;
        float yMult = v.z * w.x - v.x * w.z;
        float zMult = v.x * w.y - v.y * w.x;

        Vector3 crossProd = new Vector3(xMult, yMult, zMult);
        return crossProd;
    }

    private void CalculateAngle()
    {
        Vector3 pFrwd = this.gameObject.transform.up;
        Vector3 rDir = end.transform.position - this.gameObject.transform.position;

        float dot = pFrwd.x * rDir.x + pFrwd.y * rDir.y;
        float angle = Mathf.Acos(dot / (pFrwd.magnitude * rDir.magnitude));

        //Debug.Log("Angle: " + angle * Mathf.Rad2Deg);
        //Debug.Log("Unity angle: " + Vector3.Angle(pFrwd, rDir));

        //Debug.DrawRay(this.transform.position, pFrwd * 15, Color.green, 2);
        //Debug.DrawRay(this.transform.position, rDir, Color.red, 2);

        int clockWise = 1;
        if (Cross(pFrwd, rDir).z < 0)
        {
            clockWise = -1;
        }

        //Unity calculation on the angle
        float unityAngle = Vector3.SignedAngle(pFrwd, rDir, this.gameObject.transform.forward);
        //Debug.Log("forward: " + this.transform.forward);

        this.gameObject.transform.Rotate(0, 0, unityAngle * 0.02f);

        //this.transform.Rotate(0, 0, angle * Mathf.Rad2Deg * clockWise);
    }
    private void ProgressTracker()
    {
        if (Vector3.Distance(tracker.transform.position, this.gameObject.transform.position) > 0.8f)
        {
            //float dis = Vector3.Distance(tracker.transform.position, startObject.transform.position);
            return;
        }

        if (currentWP > 0 && Vector3.Distance(tracker.transform.position, waypoints[currentWP].transform.position) < 0.05f)
        {
            currentWP--;
        }

        if (currentWP == 0 && Vector3.Distance(tracker.transform.position, end.transform.position) < 0.35f)
        {
            done = false;
            f_Pushed = false;
            //currentWP = waypoints.Count - 1;
        }
        tracker.transform.LookAt(waypoints[currentWP].transform);
        tracker.transform.Translate(0, 0, (speed + 0.5f) * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (placePlatformButtons != null)
        {
            foreach (PlacePlatformHandler p in placePlatformButtons)
            {
                if (p.searchPathAgain == true)
                {
                    Re_Search();
                    p.searchPathAgain = false;
                }
            }
        }

        if (searching == true)
        {
            Search(lastPosition);
        }

        if (done == true)
        {
            GetPath();
            done = false;
            if (f_Pushed == false)
            {
                f_Pushed = true;
            }
        }
        //if (Input.GetKeyDown(KeyCode.K) && f_Pushed == false)
        //{
        //    f_Pushed = true;
        //}

        if (this.gameObject != null && f_Pushed == true && waypoints.Count > 0)
        {

            //CalculateAngle();
            //start.transform.Translate(start.transform.up * autoSpeed, Space.World);
            if (tracker != null)
            {
                ProgressTracker();
            }
            // Debug.Log("bbbb ?");
            // Vector3 myLocation = this.gameObject.transform.position;
            //  Vector3 targetLocation = waypoints[currentWP].transform.position;

            Vector3 direction = (tracker.transform.position - this.gameObject.transform.position);



            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * direction;
            Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);

            this.gameObject.transform.localRotation = Quaternion.Slerp(this.gameObject.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            this.gameObject.transform.Translate(speed * Time.deltaTime, 0, 0);

            //Debug.DrawRay(start.transform.position, direction, Color.red);
            //Debug.DrawRay(start.transform.position, rotatedVectorToTarget, Color.green);
            //Debug.Log("The quaternion -  " + targetRotation);
            //Debug.Log("Green Y, Upwards vector " + rotatedVectorToTarget);
            //Debug.Log("Direction X, to target vector " + direction);
            //Debug.Log("Forward vector " + Vector3.forward);
            //Debug.DrawRay(start.transform.position, Vector3.up * 5, Color.yellow);
            //Debug.DrawRay(start.transform.position, Vector3.forward * 5, Color.white);
            //Debug.DrawRay(this.transform.position, this.transform.right * 2, Color.red);
        }


        Debug.Log("waypoints = " + waypoints.Count);
    }
    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, transform.right, 2f, wallLayer);
        //if (hit.collider != null)
        //{
        //    Debug.Log("collders name = " + hit.collider.name);
        //}
        if (hit.collider != null && !mapGround.Contains(hit.collider))
        {
            Re_Search();
            mapGround.Add(hit.collider);

        }

    }

    private void Re_Search()
    {
        maze.MarkTheGround();
        f_Pushed = false;
        done = true;
        searching = false;

        BeginSearch();

        if (!done)
        {
            searching = true;
        }
    }

    //private void OnTriggerEnter2D(Collider2D otherObject)
    //{
    //    int layerName = LayerMask.NameToLayer("GroundLayer");
    //    if (otherObject.gameObject.layer == layerName)
    //    {
    //        maze.MarkTheGround();
    //        f_Pushed = false;
    //        done = true;
    //        searching = false;

    //        BeginSearch();

    //        if (!done)
    //        {
    //            searching = true;
    //        }
    //        Debug.Log("How many tim,es");
    //    }
    //}
}
