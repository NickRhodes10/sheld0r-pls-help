using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WFC
{
    [CreateAssetMenu(menuName = "WFC/Module")]
    public class Module : ScriptableObject
    {
        public TileBase tilebase;
        public Sprite image;

        public Module[] north;
        public Module[] east;
        public Module[] south;
        public Module[] west;

        public Module[] detailModule;
    }
}
