using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WFC
{
    [CreateAssetMenu(menuName = "WFC/2DTileset")]
    public class TwoDimensionalTileset : ScriptableObject
    {
        public Module[] wfcModules;
        public Module[] detailModules;
    }
}
