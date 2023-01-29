using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DataStorage;

[CustomEditor(typeof(Database))]
public class Database_Editor : Editor
{
    private Database _curDatabase;

    private void OnEnable()
    {
        _curDatabase = target as Database;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Set Index"))
        {
            _curDatabase.SetIndexes();

            for (int i = 0; i < _curDatabase.elements.Length; i++)
            {
                 EditorUtility.SetDirty(_curDatabase.elements[i]);
            }
        }
    }
}
