using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapGeneration;
using UnityEngine.Tilemaps;

public class FinalTilemapGeneration : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _3DWall;
    [SerializeField] private TileBase _3DFloor;

    [SerializeField] private Tilemap _2DTilemap;

}
