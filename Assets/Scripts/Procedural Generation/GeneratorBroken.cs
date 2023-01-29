/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathsHelper;
using CollisionHelper;
using Delaunay;
using UnityEngine.Tilemaps;
using WFC;

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

        public bool runWFC = false;

        private List<Room> _gizmoRooms = new List<Room>();
        private List<Edge> _gizmoEdges = new List<Edge>();
        private List<Edge> _gizmoMST = new List<Edge>();

        public class RoomData
        {
            public Vector2Int RoomPos;
            public Vector2Int RoomSize;
        }

        private void Start()
        {
            Generate();
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

        private IEnumerator PrimMinimumSpanning(List<Edge> edges, List<Room> rooms)
        {
            List<Edge> mst = new List<Edge>();

            List<Room> unreached = new List<Room>(rooms);

            for (int i = unreached.Count - 1; i >= 0; i--)
            {
                if (unreached[i].TurnedOff == true)
                {
                    unreached.RemoveAt(i);
                }
            }

            List<Room> reached = new List<Room>();

            Dictionary<Room, List<Edge>> roomDict = new Dictionary<Room, List<Edge>>();

            for (int i = 0; i < rooms.Count; i++)
            {
                List<Edge> edgesFound = DelaunayCalculator.CollectAllEdgesConnectedToPoint(edges, rooms[i].Position);

                if (roomDict.ContainsKey(rooms[i]) == false)
                {
                    roomDict.Add(rooms[i], edgesFound);
                }
                else
                {
                    roomDict[rooms[i]] = edgesFound;
                }
            }

            reached.Add(unreached[0]);
            unreached.RemoveAt(0);

            while (unreached.Count > 0)
            {
                float curBestDistance = Mathf.Infinity;

                int reachedIndex = 0;
                int unreachedIndex = 0;
                int localEdgeIndex = 0;
                Edge curEdge = null;

                for (int i = 0; i < reached.Count; i++)
                {
                    for (int j = 0; j < unreached.Count; j++)
                    {
                        Room reachedRoom = reached[i];
                        Room unreachedRoom = unreached[j];

                        List<Edge> edgesOfRoom = roomDict[reachedRoom];

                        for (int e = 0; e < edgesOfRoom.Count; e++)
                        {
                            if (edgesOfRoom[e].SharesEdge(reachedRoom.Position, unreachedRoom.Position) && curBestDistance > edgesOfRoom[e].GetDistance)
                            {
                                curBestDistance = edgesOfRoom[e].GetDistance;
                                reachedIndex = i;
                                unreachedIndex = j;
                                curEdge = edgesOfRoom[e];
                                localEdgeIndex = e;
                            }
                        }
                    }
                }

                if (curEdge != null)
                {
                    Room reachedNode = reached[reachedIndex];
                    Room unreachedNode = unreached[unreachedIndex];

                    for (int i = roomDict[unreachedNode].Count - 1; i >= 0; i--)
                    {
                        if (roomDict[unreachedNode][i].SharesEdge(unreachedNode.Position, reachedNode.Position))
                        {
                            roomDict[unreachedNode].RemoveAt(i);
                        }
                    }

                    roomDict[reachedNode].RemoveAt(localEdgeIndex);
                    reached.Add(unreachedNode);
                    unreached.RemoveAt(unreachedIndex);
                    mst.Add(curEdge);
                    edges.Remove(curEdge);

                }

                yield return null;

            }

            AddRandomEdges(edges, mst);
            List<Edge> corridors = CalculateCorridors(mst, rooms);
            StartCoroutine(DrawContent(rooms, corridors));
            _gizmoMST = mst;


        }

        private IEnumerator DrawContent(List<Room> rooms, List<Edge> corridors)
        {
            int iteration = 0;

            for (int i = 0; i < rooms.Count; i++)
            {
                Room curRoom = rooms[i];

                //if (WFCGenerator.instance != null)
                //WFCGenerator.instance.rooms.Add(rooms[i]);

                if (curRoom.TurnedOff == false)
                {
                    Vector2 topLeft = new Vector2(curRoom.Position.x - curRoom.GetWidth, curRoom.Position.y + curRoom.GetHeight);
                    Vector2 topRight = new Vector2(curRoom.Position.x + curRoom.GetWidth, curRoom.Position.y + curRoom.GetHeight);
                    Vector2 bottomLeft = new Vector2(curRoom.Position.x - curRoom.GetWidth, curRoom.Position.y - curRoom.GetHeight);
                    Vector2 bottomRight = new Vector2(curRoom.Position.x + curRoom.GetWidth, curRoom.Position.y - curRoom.GetHeight);

                    MapVisualController.RectFill(curRoom.Position, curRoom.GetWidth, curRoom.GetHeight, _floorTile, _tilemap);

                    MapVisualController.RectFill(topLeft, topRight, 1, _wallTile, _tilemap);
                    MapVisualController.RectFill(topLeft, bottomLeft, 1, _wallTile, _tilemap);
                    MapVisualController.RectFill(bottomLeft, bottomRight, 1, _wallTile, _tilemap);
                    MapVisualController.RectFill(bottomRight, topRight, 1, _wallTile, _tilemap);

                    if (WFCGenerator.instance != null)
                    {
                        Vector2Int pos = Vector2Int.RoundToInt(curRoom.Position);
                        pos.x -= curRoom.GetWidth;
                        pos.y -= curRoom.GetHeight;
                        Vector2Int size = Vector2Int.RoundToInt(curRoom.GetSize);

                        //WFCGenerator.instance.AddCurRoomData(pos, size);

                        //var dictionary = new WFCGenerator.RoomDictionary();
                        //dictionary.Add(i, pos, size);
                    }

                    /*
                    if (runWFC)
                    {
                        if (WFCGenerator.instance != null)
                        {
                            Vector2Int pos = Vector2Int.RoundToInt(curRoom.Position);
                            pos.x -= curRoom.GetWidth;
                            pos.y -= curRoom.GetHeight;
                            Vector2Int size = Vector2Int.RoundToInt(curRoom.GetSize);
                            WFCGenerator.instance.Generate(pos, size);
                        }
                    }
                    

                    iteration++;

                    if (iteration >= _refreshCounterMax)
                    {
                        iteration = 0;
                        yield return null;
                    }


                }
            }

            for (int i = 0; i < corridors.Count; i++)
            {
                Edge curEdge = corridors[i];
                MapVisualController.RectFill(curEdge.GetPointA, curEdge.GetPointB, 1, _floorTile, _tilemap, true);
                MapVisualController.RectFill(curEdge.GetPointA, curEdge.GetPointB, 2, _wallTile, _tilemap, false);

                iteration++;

                if (iteration >= _refreshCounterMax)
                {
                    iteration = 0;
                    if (WFCGenerator.instance != null)
                    {

                    }
                    yield return null;
                }
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
*/

