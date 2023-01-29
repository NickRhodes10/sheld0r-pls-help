using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataStorage
{
    [CreateAssetMenu(menuName = "Database")]
    public class Database : ScriptableObject
    {
        public DatabaseElement[] elements;


        //Saves Scriptable Objects to an int
        public void SetIndexes()
        {
            for (int i = 0; i < elements.Length; i++)
            {
                elements[i].SetIndex(i);
            }
        }
    }
}