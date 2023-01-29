using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WFC
{
    [CustomEditor(typeof(Tileset))]
    public class Tileset_Editor : Editor
    {
        private Tileset _tileset;

        private void OnEnable()
        {
            _tileset = target as Tileset;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(GUILayout.Button("Generate Neighbos"))
            {
                _tileset.SetNeighbors();
            }
        }
    }
}
