/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using MapGeneration;

namespace WFC
{
    public class WFCGenerator : MonoBehaviour
    {
        //[SerializeField] private Vector2Int _worldSize;
        [SerializeField] private Tileset _tileset;
        [SerializeField] private Tilemap _tilemap;

        public Dictionary<int, RoomData> RoomDictionary;

        public static WFCGenerator instance;

        private void Awake()
        {
            if (WFCGenerator.instance == null)
            {
                WFCGenerator.instance = this;
            }
            else if (WFCGenerator.instance != this)
            {
                Destroy(this);
            }
        }

        public void AddCurRoomData(Vector2Int curRoomPos, Vector2Int curRoomSize)
        {
            RoomData.RoomPos = curRoomPos;
            RoomData.RoomSize = curRoomSize;
        }
        public class RoomData
        {
            public static Vector2Int RoomPos;
            public static Vector2Int RoomSize;
        }
        public void SaveRoomDataToDictionary(int index)
        {
            RoomDictionary.Add(new KeyValuePair<int, RoomData>())
            //RoomDictionary = new Dictionary<int, RoomData>
        }

        /*
        public class RoomDictionary : Dictionary<int, RoomData>
        {
            public void Add(int roomsIndex, Vector2Int position, Vector2Int size)
            {
                RoomData data;
                data.RoomPos = position;
                data.RoomSize = size;
                this.Add(roomsIndex, data);
            }            
        }
        // 


        public void Generate()
        {
            foreach (var item in RoomDictionary)
            {

                StartCoroutine(CreateWorld(position, size));
            }
        }

        private IEnumerator CreateWorld(Vector2Int position, Vector2Int size)
        {
            Element[,] grid = new Element[size.x, size.y];
            List<Vector2Int> unreachedPositions = new List<Vector2Int>();

            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    Vector2Int newPosition = new Vector2Int(x, y);
                    grid[x, y] = new Element(_tileset.modules, newPosition, position);
                    unreachedPositions.Add(new Vector2Int(x, y));
                }
            }

            int rng = Random.Range(0, unreachedPositions.Count);

            CollapseElement(grid[unreachedPositions[rng].x, unreachedPositions[rng].y], grid);
            unreachedPositions.RemoveAt(rng);

            while (unreachedPositions.Count > 0)
            {

                Element curElement;
                List<Element> lowEntropyElements = new List<Element>();
                int lowestEntropy = int.MaxValue;

                for (int i = 0; i < unreachedPositions.Count; i++)
                {
                    curElement = grid[unreachedPositions[i].x, unreachedPositions[i].y];
                    if (curElement.GetEntropy < lowestEntropy)
                    {
                        lowestEntropy = curElement.GetEntropy;
                        lowEntropyElements.Clear();
                        lowEntropyElements.Add(curElement);
                    }
                    else if (curElement.GetEntropy == lowestEntropy)
                    {
                        lowEntropyElements.Add(curElement);
                    }
                }

                rng = Random.Range(0, lowEntropyElements.Count);
                curElement = lowEntropyElements[rng];

                CollapseElement(curElement, grid);
                unreachedPositions.Remove(curElement.GetPosition);

                yield return null;
            }
        }

        private void CollapseElement(Element curElement, Element[,] grid)
        {
            if (curElement == null)
            {
                return;
            }

            curElement.Collapse(_tilemap);

            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    if ((x == 0 && y == 0) || (Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1))
                    {
                        continue;
                    }

                    int curX = curElement.GetPosition.x + x;
                    int curY = curElement.GetPosition.y + y;

                    if ((curX < 0 || curY < 0) || curX > grid.GetLength(0) - 1 || curY > grid.GetLength(1) - 1)
                    {
                        continue;
                    }

                    Element curNeighbor = grid[curX, curY];

                    if (curNeighbor == null)
                    {
                        return;
                    }

                    if (x > 0) //Right
                    {
                        curNeighbor.RemoveOptions(curElement.GetSelectedModule.east);
                    }
                    else if (x < 0) //Left
                    {
                        curNeighbor.RemoveOptions(curElement.GetSelectedModule.west);
                    }
                    else if (y > 0) //Up
                    {
                        curNeighbor.RemoveOptions(curElement.GetSelectedModule.north);
                    }
                    else if (y < 0) //Down
                    {
                        curNeighbor.RemoveOptions(curElement.GetSelectedModule.south);
                    }
                }
            }
        }
    }

    public class Element
    {
        private List<Module> _options;
        private Vector2Int _position;
        private Module _selectedModule;
        private Vector2Int _offset;

        private int _entropy;

        public Vector2Int GetPosition { get { return _position; } }
        public Module GetSelectedModule { get { return _selectedModule; } }
        public int GetEntropy { get { return _options.Count; } }

        public Element(List<Module> options, Vector2Int position, Vector2Int offset)
        {
            _options = options;
            _position = position;
            _offset = offset;
        }

        public Element(Module[] options, Vector2Int position, Vector2Int offset)
        {
            _options = new List<Module>(options);
            _position = position;
            _offset = offset;
        }

        public void RemoveOptions(Module[] legalNeighbors)
        {
            List<Module> temp = new List<Module>(legalNeighbors);

            for (int i = _options.Count - 1; i >= 0; i--)
            {
                if (temp.Contains(_options[i]) == false)
                {
                    _options.RemoveAt(i);
                }
            }
        }

        public void Collapse(Tilemap tilemap)
        {
            if (_options.Count == 0)
            {
                Debug.Log("Nope");
                return;
            }

            int rng = Random.Range(0, _options.Count);
            _selectedModule = _options[rng];

            Vector3Int offsetPosition = (Vector3Int)_position + (Vector3Int)_offset;
            tilemap.SetTile(offsetPosition, _selectedModule.tilebase);

            //tilemap.SetTile((Vector3Int)_position, _selectedModule.tilebase);
        }
    }
}
*/