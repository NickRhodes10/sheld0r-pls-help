using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathsHelper;
using UnityEngine.Tilemaps;
using MapGeneration;
public class SpaceManager : MonoBehaviour
{
    public static SpaceManager instance;

    public Vector3 spaceSize;
    public LayerMask layerMask;
    public float maxDistance;
    public Tilemap Map;

    [Range(0f, 100f)] public float radius = 1f;

    public SpaceStorage[,] _spaces;


    [SerializeField]private Transform _player;
    private void Awake()
    {
        if (SpaceManager.instance == null)
        {
            SpaceManager.instance = this;
        }
        else if (SpaceManager.instance != this)
        {
            Destroy(this);
        }
    }


    private void FixedUpdate()
    {
        if(_spaces == null || _spaces.Length == 0)
        {
            return;
        }

        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (_player == null)
        {
            return;
        }

        Debug.Log("X: " + _spaces.GetLength(0) + ", Y: " + _spaces.GetLength(1));

        for (int x = 0; x < _spaces.GetLength(0); x++)
        {
            for (int y = 0; y < _spaces.GetLength(1); y++)
            {
                _spaces[x, y].Visualize(Vector3.Distance(_player.position, _spaces[x,y].transform.position) <= radius);
            }
        }
    }


    public void CalculateSpace()
    {
        if (maxDistance == 0 || Map == null)
        {
            Debug.Log("Please do not divide by 0");
            return;
        }

        Map.ResizeBounds();

        Debug.Log(Map.localBounds.min.x + "," + Map.localBounds.min.z);
        Debug.Log(Map.localBounds.max.x + "," + Map.localBounds.max.z);

        float xRange = GenericNumbers.Distance(Map.localBounds.min.x, Map.localBounds.max.x);
        float yRange = GenericNumbers.Distance(Map.localBounds.min.z, Map.localBounds.max.z);

        xRange = Mathf.CeilToInt(xRange / maxDistance);
        yRange = Mathf.CeilToInt(yRange / maxDistance);

        _spaces = new SpaceStorage[(int)xRange, (int)yRange];

        Vector3 startPos = new Vector3(Map.localBounds.min.x, 0f, Map.localBounds.min.z);

        for (int x = 0; x < _spaces.GetLength(0); x++)
        {
            for (int y = 0; y < _spaces.GetLength(1); y++)
            {
                Vector3 newPosition = startPos + new Vector3(x * maxDistance, 0f, y * maxDistance);
                GameObject go = new GameObject("x:" + x + " y:" + y);
                go.SetActive(false);
                go.transform.position = newPosition;
                _spaces[x, y] = go.AddComponent<SpaceStorage>();
                go.SetActive(true);
                go.GetComponent<SpaceStorage>().FillTransforms();
            }
        }
    }
}
