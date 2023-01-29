using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WFC
{
    [CreateAssetMenu(menuName = "WFC/3DTileset")]
    public class ThreeDeeTileset : ScriptableObject
    {
        public RuleTile[] ruleTiles;
    }
}
