using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathsHelper;
using CollisionHelper;
using Delaunay;
using UnityEngine.Tilemaps;
using WFC;
using LevelData;
using MapGeneration;

namespace MapGeneration
{
    public class Generator : MonoBehaviour
    {
        [SerializeField] private UIntRange _numberOfRooms;
        [SerializeField] private UIntRange _roomSize;
        [SerializeField] private UIntRange _seperationRange;
        [SerializeField] private int _refreshCounterMax = 50;
        [SerializeField, Range(0, 1)] private float _cullingPercent;
        [SerializeField] private float _superTriangleOffset;
        [SerializeField, Range(0, 1)] private float _edgeReconnectionPercent;

        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TileBase _floorTile;
        [SerializeField] private TileBase _wallTile;
        [SerializeField] private Tileset _wfcTileset;

        [SerializeField] private Tilemap _3Dtilemap;
        [SerializeField] private TileBase _3DfloorTile;
        [SerializeField] private TileBase _3DwallTile;

        private List<Room> _gizmoRooms = new List<Room>();
        private List<Edge> _gizmoEdges = new List<Edge>();
        private List<Edge> _gizmoMST = new List<Edge>();
        private void Awake()
        {
            if (_wfcTileset != null)
            {
                _wfcTileset.SetNeighbors();
            }
        }

        private void Start()
        {
            Generate();
            if (_wfcTileset != null)
            {
                _wfcTileset.SetNeighbors();
            }
        }

        public void Generate()
        {
            List<Room> rooms = new List<Room>();

            for (int i = 0; i < _numberOfRooms.GetRandomIntValue; i++)
            {
                // Add rooms to list using the inspector values entered for dimensions, place at origin 0,0,0.
                rooms.Add(new Room(_roomSize.GetRandomUintValue, _roomSize.GetRandomUintValue, _roomSize.GetRandomUintValue, Vector3.zero));
            }

            _gizmoRooms = rooms;

            StartCoroutine(SeperateRooms(rooms));
        }

        private IEnumerator SeperateRooms(List<Room> rooms)
        {
            bool collisionFound = true;

            while (collisionFound == true)
            {
                collisionFound = Collision3D.DetectRoomCollision(rooms, _seperationRange);
                yield return null;
            }

            StartCoroutine(Triangulate(rooms));
        }

        private IEnumerator Triangulate(List<Room> rooms)
        {
            rooms.Sort(SortBySize);
            TurnOffRoomsUnderSizeLimit(rooms);
            Vector2[] extents = FindExtents(rooms);

            float offsetAmount = _superTriangleOffset * _numberOfRooms.GetMaxValue;

            extents[0] += new Vector2(0f, offsetAmount);
            extents[1] += new Vector2(-offsetAmount, -offsetAmount);
            extents[2] += new Vector2(offsetAmount, -offsetAmount);

            List<Triangle> triangles = new List<Triangle>();
            triangles.Add(new Triangle(extents[0], extents[1], extents[2])); // Create super triangle using the extents as each point.

            for (int i = 0; i < rooms.Count; i++) // iterate through all rooms within list
            {
                if (rooms[i].TurnedOff == true) // Check if room is turned off, if so, continue
                {
                    continue;
                }

                triangles = DelaunayCalculator.BowyerWatson(triangles, rooms[i]); // Pass in super triangle and room @ current index of rooms list
                yield return null;
            }

            List<Edge> edges = DelaunayCalculator.CalculateEdges(triangles, extents); //!!!!!!!EDGES POPULATING EMPTY!!!!!!!!! triangles not set to an instance of an object Line 63 Deluanay
            _gizmoEdges = edges;

            StartCoroutine(PrimMinimumSpanning(edges, rooms));

        }

        /// <summary>
        /// PrimMinimumSpanning() is an algorithm that finds the shortest path from point to point
        /// without any looping. In this method, we are using edges and room lists to iterate. 
        /// </summary>
        /// <param name="edges"></param>
        /// <param name="rooms"></param>
        /// <returns></returns>
        private IEnumerator PrimMinimumSpanning(List<Edge> edges, List<Room> rooms)
        {
            List<Edge> mst = new List<Edge>();                                                                          // Create a new list for the Minimum Spanning Tree (mst)

            List<Room> unreached = new List<Room>(rooms);                                                               // Create a new list of rooms to represent rooms unreached by the algorithm 

            for (int i = unreached.Count - 1; i >= 0; i--)                                                              // Reverse for loop, starting from the end of the list,
            {
                if (unreached[i].TurnedOff == true)                                                                     // Check if the room at the current index is turned off, if so,
                {
                    unreached.RemoveAt(i);                                                                              // Remove this room from the unreached list
                }
            }

            List<Room> reached = new List<Room>();                                                                      // Create a new list of rooms to represent the rooms the algorithm has already reached

            Dictionary<Room, List<Edge>> roomDict = new Dictionary<Room, List<Edge>>();                                 // Create a new Dictionary with a type Room as a key and List of edges as a value

            for (int i = 0; i < rooms.Count; i++)                                                                       // For loop, each room in rooms list
            {
                List<Edge> edgesFound = DelaunayCalculator.CollectAllEdgesConnectedToPoint(edges, rooms[i].Position);   // List edgesFound saved the returned results from Delaunay

                if (roomDict.ContainsKey(rooms[i]) == false)                                                            // If the roomDictionary does not contain the currently evaluated room,
                {
                    roomDict.Add(rooms[i], edgesFound);                                                                 // Add it to the dictionary
                }
                else
                {
                    roomDict[rooms[i]] = edgesFound;                                                                    // Else set the current index of roomDict to be equal to the value returned by Delaunay
                }
            }

            reached.Add(unreached[0]);                                                                                  // Now we can add the evaluated room to the reached list
            unreached.RemoveAt(0);                                                                                      // Remove the evaluated room from the unreached list

            while (unreached.Count > 0)                                                                                 // While rooms are still unreached,
            {
                float curBestDistance = Mathf.Infinity;                                                                 // Set our current best distance to be infinite, this way any number is less than the beginning value

                int reachedIndex = 0;                                                                                   // Create reachedIndex variable to iterate through the lists
                int unreachedIndex = 0;                                                                                 // Create unreachedIndex to iterate through lists
                int localEdgeIndex = 0;                                                                                 // Create localEdgeIndex to iterate though lists
                Edge curEdge = null;                                                                                    // Create curEdge to track which edge is being evaluated

                for (int i = 0; i < reached.Count; i++)                                                                 // For the rooms in the reached list,
                {
                    for (int j = 0; j < unreached.Count; j++)                                                           // For the rooms in the unreached list,
                    {
                        Room reachedRoom = reached[i];                                                                  // Set reachedRoom to be equal to the value at index i of our reached list used previously
                        Room unreachedRoom = unreached[j];                                                              // Do the same for unreached rooms

                        List<Edge> edgesOfRoom = roomDict[reachedRoom];                                                 // Pull the value from the roomDictionary with the key of 'reachedRoom'
                        
                        for (int e = 0; e < edgesOfRoom.Count; e++)                                                     // For the rooms in the edgesOfRoom list,
                        {
                            if (edgesOfRoom[e].SharesEdge(reachedRoom.Position, unreachedRoom.Position)                 // If the edge at index e shares an edge with our reached room or unreached room
                                && curBestDistance > edgesOfRoom[e].GetDistance)                                        // AND the current best distance is greater than the distance of the edge at current index,
                            {
                                curBestDistance = edgesOfRoom[e].GetDistance;                                           // Assign the currently evaluated room as the currentBestDistance for comparison in next iteration
                                reachedIndex = i;                                                                       // Assign reachedIndex to the value of i, which is incrimented by 1 each loop.
                                unreachedIndex = j;                                                                     // Assign unreachedIndex to the value of j, which is incrimented by 1 each loop.
                                curEdge = edgesOfRoom[e];                                                               // Assign the curEdge to the currently evaluated index of edgesOfRoom list.
                                localEdgeIndex = e;                                                                     // Assign the localEdgeIndex to the value of e, which is incrimented by 1 each loop.
                            }
                        }
                    }
                }

                if (curEdge != null)                                                                                    // If we do not have a current edge,
                {
                    Room reachedNode = reached[reachedIndex];                                                           // Create a reachedNode variable of type Room to store the room value from reached list at index reachedIndex
                    Room unreachedNode = unreached[unreachedIndex];                                                     // Create an unreachedNode variable of type Room to store the room value from unreached list at index unreachedIndex

                    for (int i = roomDict[unreachedNode].Count - 1; i >= 0; i--)                                        // Reverse for loop to iterate backwards,
                    {
                        if (roomDict[unreachedNode][i].SharesEdge(unreachedNode.Position, reachedNode.Position))        // If the value at key unreachedNode @ index i shares an edges,
                        {
                            roomDict[unreachedNode].RemoveAt(i);                                                        // Remove the unreachedNode from the roomDictionary
                        }
                    }

                    roomDict[reachedNode].RemoveAt(localEdgeIndex);                                                     // Remove the localEdgeIndex from the roomDictionary at the reachedNode index
                    reached.Add(unreachedNode);                                                                         // Add the now evaluated unreachedNode to the reached room list
                    unreached.RemoveAt(unreachedIndex);                                                                 // Remove the now evaluated unreachedIndex from the unreached room list
                    mst.Add(curEdge);                                                                                   // Add the currentEdge to the minimum spanning tree list
                    edges.Remove(curEdge);                                                                              // Remove the currentEdge from the list of edges

                }

                yield return null;                                                                                      // Coroutine must have yield return, but we dont want to wait so just return null

            }

            AddRandomEdges(edges, mst);                                                                                 // Not part of Prim's algorithm but used to add in more edges to hallways
            List<Edge> corridors = CalculateCorridors(mst, rooms);                                                      // Generate corridors using mst and rooms as parameters
            StartCoroutine(DrawContent(rooms, corridors));                                                              // Begin delaunay drawing
            StartAndEnd.instance.FindStartAndEnd(rooms);                                                                // Idk
            _gizmoMST = mst;                                                                                            // Creates yellow gizmo representing mst pathing
        }

        private IEnumerator DrawContent(List<Room> rooms, List<Edge> corridors)
        {
            List<Room> curWFCRooms = new List<Room>();            

            for (int i = 0; i < rooms.Count; i++)
            {
                Room curRoom = rooms[i];                

                Vector2Int intPosition = Vector2Int.RoundToInt(curRoom.Position); // Finds the integer value of the current room's position

                Vector2Int topLeft = new Vector2Int(intPosition.x - curRoom.GetWidth, intPosition.y + curRoom.GetHeight);
                Vector2Int topRight = new Vector2Int(intPosition.x + curRoom.GetWidth, intPosition.y + curRoom.GetHeight);
                Vector2Int bottomLeft = new Vector2Int(intPosition.x - curRoom.GetWidth, intPosition.y - curRoom.GetHeight);
                Vector2Int bottomRight = new Vector2Int(intPosition.x + curRoom.GetWidth, intPosition.y - curRoom.GetHeight); // The bounds of the current room, used in drawing the walls

                if (curRoom.TurnedOff == false)
                {
                    curWFCRooms.Add(curRoom); // Adds the current rooms to the list of rooms to perform WFC on top of

                    GenerationStages.instance.roomsGenerated++;

                    // Draws the room floors
                    MapVisualController.RectFill(intPosition, curRoom.GetWidth, curRoom.GetHeight, _floorTile, _tilemap);
                    MapVisualController.RectFill(intPosition, curRoom.GetWidth, curRoom.GetHeight, _3DfloorTile, _3Dtilemap);                                 

                    // Then draws the room walls
                    MapVisualController.RectFill(topLeft, topRight, 1, _wallTile, _tilemap);                    
                    MapVisualController.RectFill(topLeft, bottomLeft, 1, _wallTile, _tilemap);                    
                    MapVisualController.RectFill(bottomLeft, bottomRight, 1, _wallTile, _tilemap);                    
                    MapVisualController.RectFill(bottomRight, topRight, 1, _wallTile, _tilemap);
                    
                    MapVisualController.RectFill(topLeft, topRight, 1, _3DwallTile, _3Dtilemap);
                    MapVisualController.RectFill(topLeft, bottomLeft, 1, _3DwallTile, _3Dtilemap);
                    MapVisualController.RectFill(bottomLeft, bottomRight, 1, _3DwallTile, _3Dtilemap);
                    MapVisualController.RectFill(bottomRight, topRight, 1, _3DwallTile, _3Dtilemap);
                }
            }

            // Draws the corridor floors and walls
            for (int i = 0; i < corridors.Count; i++)
            {
                Edge curEdge = corridors[i];
                MapVisualController.RectFill(curEdge.GetPointA, curEdge.GetPointB, 1, _floorTile, _tilemap, true);
                MapVisualController.RectFill(curEdge.GetPointA, curEdge.GetPointB, 1, _3DfloorTile, _3Dtilemap, true);
                MapVisualController.RectFill(curEdge.GetPointA, curEdge.GetPointB, 2, _wallTile, _tilemap, false);
                MapVisualController.RectFill(curEdge.GetPointA, curEdge.GetPointB, 2, _3DwallTile, _3Dtilemap, false);
            }

            // Performs WFC
            for (int i = 0; i < curWFCRooms.Count; i++)
            {
                Room curRoom = curWFCRooms[i];

                if (WFCGenerator.instance != null)
                {
                    Vector2Int pos = Vector2Int.RoundToInt(curRoom.Position);
                    pos.x -= curRoom.GetWidth;
                    pos.y -= curRoom.GetHeight;
                    Vector2Int size = new Vector2Int(curRoom.GetWidth * 2, curRoom.GetHeight * 2);
                    WFCGenerator.instance.Generate(pos, size);
                }

                yield return null;
            }
        }

        private void AddRandomEdges(List<Edge> edges, List<Edge> mst)
        {
            for (int i = 0; i < edges.Count; i++)
            {
                float rng = Random.Range(0f, 1f);

                if (rng < _edgeReconnectionPercent)
                {
                    mst.Add(edges[i]);
                }
            }
        }

        private List<Edge> CalculateCorridors(List<Edge> mst, List<Room> rooms)
        {
            List<Edge> corridors = new List<Edge>();

            for (int i = 0; i < mst.Count; i++)
            {
                for (int x = 0; x < rooms.Count; x++)
                {
                    Vector2 A = mst[i].GetPointA;
                    Vector2 B = mst[i].GetPointB;

                    if (rooms[x].TurnedOff == true)
                    {
                        if (CollisionHelper.Collision3D.LineIntersectRoomBounds(A, B, rooms[x]) == true)
                        {
                            rooms[x].TurnedOff = false;
                        }
                    }
                    else
                    {
                        Vector2 midPoint = new Vector2(A.x, B.y);
                        corridors.Add(new Edge(A, midPoint));
                        corridors.Add(new Edge(midPoint, B));
                    }
                }
            }

            return corridors;
        }

        private int SortBySize(Room A, Room B)
        {
            if (A.GetTotalSize < B.GetTotalSize)
            {
                return 1;
            }

            if (A.GetTotalSize > B.GetTotalSize)
            {
                return -1;
            }

            return 0;
        }

        private void TurnOffRoomsUnderSizeLimit(List<Room> rooms)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (((float)i / (float)rooms.Count) > _cullingPercent)
                {
                    rooms[i].TurnedOff = true;
                }
            }
        }

        private Vector2[] FindExtents(List<Room> rooms)
        {
            Vector2[] extents = new Vector2[3];


            for (int i = 0; i < extents.Length; i++) // Set all rooms within extents array to default value (0,0,0)
            {
                extents[i] = Vector2.zero; // Assign default value;
            }

            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].Position.y > extents[0].y)
                {
                    extents[0] = rooms[i].Position;
                }

                if (rooms[i].Position.y < extents[1].y)
                {
                    if (rooms[i].Position.x < extents[1].x)
                    {
                        extents[1] = rooms[i].Position;
                    }

                }

                if (rooms[i].Position.y < extents[2].y)
                {
                    if (rooms[i].Position.x > extents[2].x)
                    {
                        extents[2] = rooms[i].Position;
                    }

                }

            }

            return extents;

        }

        // Draw room gizmos
        private void OnDrawGizmos()
        {
            if (_gizmoRooms != null)
            {
                for (int i = 0; i < _gizmoRooms.Count; i++)
                {
                    if (_gizmoRooms[i].TurnedOff == true)
                    {
                        Gizmos.color = Color.black;
                        Gizmos.DrawWireCube(_gizmoRooms[i].Position, _gizmoRooms[i].GetSize);
                    }
                    else
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawWireCube(_gizmoRooms[i].Position, _gizmoRooms[i].GetSize);
                    }
                }
            }

            if (_gizmoEdges != null)
            {
                Gizmos.color = Color.blue;

                for (int i = 0; i < _gizmoEdges.Count; i++)
                {
                    Gizmos.DrawLine(_gizmoEdges[i].GetPointA, _gizmoEdges[i].GetPointB);
                }
            }

            if (_gizmoMST != null)
            {
                Gizmos.color = Color.yellow;
                for (int i = 0; i < _gizmoMST.Count; i++)
                {
                    Gizmos.DrawLine(_gizmoMST[i].GetPointA, _gizmoMST[i].GetPointB);
                }
            }
        }

    }

    public class Room
    {
        private Vector2 _position;
        private Vector2 _size;
        private bool _turnedOff;

        private float _totalSize = -1;

        public Vector2 Position { get { return _position; } set { _position = value; } }
        public Vector2 GetSize { get { return _size; } }
        public bool TurnedOff { get { return _turnedOff; } set { _turnedOff = value; } }
        public int GetWidth { get { return (int)(_size.x * 0.5f); } }
        public int GetHeight { get { return (int)(_size.y * 0.5f); } }


        public float GetTotalSize
        {
            get
            {
                if (_totalSize == -1)
                {
                    _totalSize = _size.x * _size.y;
                }

                return _totalSize;
            }
        }

        public Room(uint width, uint height, uint depth, Vector2 position)
        {
            _size = new Vector2(width, height);
            _position = position;
            _turnedOff = false;
        }
    }


}

