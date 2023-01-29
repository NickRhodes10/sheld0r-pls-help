using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataStorage
{
    public class DatabaseElement : ScriptableObject
    {
        [SerializeField] private int _index;

        public int GetIndex { get { return _index; } }

        public void SetIndex(int i)
        {
            _index = i;
        }
    }
}