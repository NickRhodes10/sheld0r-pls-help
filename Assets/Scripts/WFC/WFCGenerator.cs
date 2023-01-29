using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using MapGeneration;

namespace WFC
{
    public class WFCGenerator : MonoBehaviour
    {
        [SerializeField] private Tileset _tileset;
        [SerializeField] private Tilemap _tilemap;

        [SerializeField] private Tilemap _detailTilemap;
        [Range(0f, 1f)] public float spawnChance = .5f;

        public static WFCGenerator instance;

        private void Awake()
        {
            if (WFCGenerator.instance == null)
            {
                WFCGenerator.instance = this;
            }
            else if(WFCGenerator.instance != this)
            {
                Destroy(this);
            }
        }        

        public void Generate(Vector2Int position, Vector2Int size)
        {
            StartCoroutine(CreateWorld(position, size));
        }


        private IEnumerator CreateWorld(Vector2Int position, Vector2Int size)
        {
            GenerationStages.instance.curWFCRooms++;
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
                    else if(curElement.GetEntropy == lowestEntropy)
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
            GenerationStages.instance.curWFCRooms--;
            GenerationStages.instance.finWFCRooms++;
        }

        private void CollapseElement(Element curElement, Element[,] grid)
        {
            if(curElement == null)
            {
                return;
            }

            curElement.SpawnChance = spawnChance;
            curElement.Collapse(_tilemap, _detailTilemap);            

            // Manages removing valid neighbors
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    if((x == 0 && y == 0) || (Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1))
                    {
                        continue;
                    }

                    int curX = curElement.GetPosition.x + x;
                    int curY = curElement.GetPosition.y + y;

                    if((curX < 0 || curY < 0) || curX > grid.GetLength(0) - 1 || curY > grid.GetLength(1) - 1)
                    {
                        continue;
                    }

                    Element curNeighbor = grid[curX, curY];

                    if(curNeighbor == null)
                    {
                        return;
                    }

                    if(x > 0) //Right
                    {
                        curNeighbor.RemoveOptions(curElement.GetSelectedModule.east);
                    }
                    else if(x < 0) //Left
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
        private List<Module> _detailModules;
        private Vector2Int _position;
        
        private Vector2Int _offset;
        private Module _selectedModule;

        private int _entropy;

        private float _spawnChance;

        public Vector2Int GetPosition { get { return _position; } }
        public Module GetSelectedModule { get { return _selectedModule; } }
        public int GetEntropy { get { return _options.Count; } }
        public float SpawnChance { get { return _spawnChance; } set { _spawnChance = value; } }

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
                if(temp.Contains(_options[i]) == false)
                {
                    _options.RemoveAt(i);
                }                
            }
        }

        public void Collapse(Tilemap tilemap, Tilemap detailTilemap)
        {
            if(_options.Count == 0)
            {
                Debug.Log("No legal options");
                return;
            }

            int rng = Random.Range(0, _options.Count);
            
            _selectedModule = _options[rng];

            Vector3Int offsetPosition = (Vector3Int)_position + (Vector3Int)_offset;
            tilemap.SetTile(offsetPosition, _selectedModule.tilebase);
            
            float random = Random.Range(0f, 1f);

            if ( _selectedModule.detailModule != null)
            {
                if (random < _spawnChance)
                {
                    if (_selectedModule.detailModule.Length != 0 && _selectedModule.detailModule != null)
                    {
                        detailTilemap.SetTile(offsetPosition, _selectedModule.detailModule[0].tilebase);
                    }
                    else
                    {
                        return;
                    }
                }
            }
            
            else { return; }
        }
    }
}
