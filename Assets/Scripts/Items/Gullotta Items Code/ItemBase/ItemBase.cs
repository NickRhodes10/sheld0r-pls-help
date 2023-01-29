using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStorage;

namespace ItemSystem
{
    public class ItemBase : DatabaseElement
    {
        public string itemName;
        public int itemLVL;
        public string itemDescription;
        public Sprite itemIcon;
        public int itemCost;
    }

    //Move token stuff, this was put here for speed but not actually where it belongs
    [System.Serializable]
    public class TokenBase
    {
        protected int _index;

        public int GetIndex { get { return _index; } }

        public void SetIndex(int i)
        {
            _index = i;
        }
    }

    [System.Serializable]
    public class ItemToken: TokenBase
    {
    }
}